using Application.Events;
using Application.IRepository;
using Domain.Entity;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Application.Services.Consumer.DTOS;
using Microsoft.Extensions.Http;
using Application.Handlers;

namespace Application.Services.Consumer
{
    public class Consumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMQConfiguration _rabbitMQConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private IConnection _connection;
        private IModel _channel;

        public Consumer( IServiceScopeFactory scopeFactory,IOptions<RabbitMQConfiguration> rabbitMQConfig,
            IHttpClientFactory httpClientFactory)
        {
            _scopeFactory = scopeFactory;
            _rabbitMQConfig = rabbitMQConfig.Value;
            _httpClientFactory = httpClientFactory;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _rabbitMQConfig.ExchangeName, type: ExchangeType.Topic, durable: true);

            _channel.QueueDeclare(queue: _rabbitMQConfig.QueueName,durable: true,exclusive: false,autoDelete: false);

            _channel.QueueBind(queue: _rabbitMQConfig.QueueName,exchange: _rabbitMQConfig.ExchangeName,
                routingKey: "account.user.registered");

            _channel.QueueBind(queue: _rabbitMQConfig.QueueName, exchange: _rabbitMQConfig.ExchangeName,
                routingKey: "library.book.#");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.BasicQos(0, 5, false);
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var routingKey = ea.RoutingKey;
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    using var scope = _scopeFactory.CreateScope();

                    switch (routingKey)
                    {
                        case "account.user.registered":
                            var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(json)
                                            ?? throw new Exception("Invalid message payload");
                            var handler1 = scope.ServiceProvider.GetRequiredService<UserRegisteredHandle>();
                            await handler1.Handle(userEvent);
                            break;

                        case "library.book.borrowed":
                            var borrowEvent = JsonSerializer.Deserialize<BorrowRequestedEvent>(json)
                                              ?? throw new Exception("Invalid message payload");
                            var handler2 = scope.ServiceProvider.GetRequiredService<BorrowRequestedHandler>();
                            await handler2.Handle(borrowEvent);
                            break;

                        case "library.book.borrow.approved":
                            var approvedEvent = JsonSerializer.Deserialize<BorrowingApprovedEvent>(json)
                                                ?? throw new Exception("Invalid message payload");
                            Console.WriteLine(json);
                            var handler3 = scope.ServiceProvider.GetRequiredService<BorrowingApprovedHandler>();
                            await handler3.Handle(approvedEvent);
                            break;

                        case "library.book.borrow.returned":
                            var returnedEvent = JsonSerializer.Deserialize<BookReturnedEvent>(json)
                                                ?? throw new Exception("Invalid message payload");
                            var handler4 = scope.ServiceProvider.GetRequiredService<BookReturnedHandler>();
                            await handler4.Handle(returnedEvent);
                            break;
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: _rabbitMQConfig.QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
 }


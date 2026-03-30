using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Application.Event;
using Domain.Entities;
using Application.IRepository;


namespace Application.Services.Users
{
    public class Consumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMQConfiguration _rabbitMQConfig;
        private IConnection _connection;
        private IModel _channel;
        public Consumer(IServiceScopeFactory scopeFactory, IOptions<RabbitMQConfiguration> rabbitMQConfig)
        {
            _scopeFactory = scopeFactory;
            _rabbitMQConfig = rabbitMQConfig.Value;
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                Port = _rabbitMQConfig.Port,
                DispatchConsumersAsync = true
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _rabbitMQConfig.ExchangeName, type: ExchangeType.Topic, durable: true);

            _channel.QueueDeclare(queue: _rabbitMQConfig.QueueName, durable: true, exclusive: false, autoDelete: false);

            _channel.QueueBind(queue: _rabbitMQConfig.QueueName, exchange: _rabbitMQConfig.ExchangeName,
                routingKey: "account.user.#");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var consumer = new EventingBasicConsumer(_channel);
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
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    switch (routingKey)
                    {
                        case "account.user.registered":
                            var created = JsonSerializer.Deserialize<UserCreatedEvent>(json) ??
                                throw new Exception("Invalid message payload");
                            
                            if (await userRepository.GetByIdAsync(created.UserId) == null)
                            {
                                await userRepository.AddAsync(new User
                                {
                                    Id = created.UserId,
                                    Name = created.UserName
                                });
                                Console.WriteLine($"User Saved");
                            }
                            break;

                        case "account.user.updated":
                            var updated = JsonSerializer.Deserialize<UserUpdatedEvent>(json) ??
                                throw new Exception("Invalid message payload");

                            var userUpdated = await userRepository.GetByIdAsync(updated.UserId);
                            if (userUpdated != null)
                            {
                                userUpdated.Name = updated.Name;
                                await userRepository.UpdateAsync(userUpdated);
                                Console.WriteLine($"User Updated");
                            }
                            break;

                        case "account.user.deleted":
                            var deleted = JsonSerializer.Deserialize<UserDeletedEvent>(json) ??
                                throw new Exception("Invalid message payload");

                            var userDeleted = await userRepository.GetByIdAsync(deleted.UserId);
                            if (userDeleted != null)
                            {
                                await userRepository.DeleteAsync(userDeleted.Id);
                                Console.WriteLine($"User deleted");
                            }
                            break;
                    }
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            };

            _channel.BasicConsume(
                queue: _rabbitMQConfig.QueueName,
                autoAck: false,
                consumer: consumer);
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

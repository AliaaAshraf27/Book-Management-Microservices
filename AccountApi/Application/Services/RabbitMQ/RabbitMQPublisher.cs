using Application.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application.Services.RabbitMQ
{
    public class RabbitMqPublisher : IRabbitMQPublisher, IDisposable
    {
        private readonly RabbitMQConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly object _lock = new();

        public RabbitMqPublisher(IOptions<RabbitMQConfiguration> options)
        {
            _config = options.Value;

            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _config.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
            );
        }

        public void Publish<T>(T message, string routingKey)
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(
                    exchange: _config.ExchangeName,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body
                );
            }
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }


}

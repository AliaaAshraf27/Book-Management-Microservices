using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(T message, string routingKey);
    }
}

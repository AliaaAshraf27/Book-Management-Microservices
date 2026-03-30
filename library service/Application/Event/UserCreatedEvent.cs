using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class UserCreatedEvent : IntegrationEvent
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class UserDeletedEvent : IntegrationEvent
    {
        public string UserId { get; set; }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();  
        public DateTime OccurredOn { get; } = DateTime.UtcNow; // وقت الحدث
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class BorrowRequestedEvent : IntegrationEvent
    {
        public Guid BorrowingId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class BorrowingApprovedEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string BookTitle { get; set; }
        public DateTime DueDate { get; set; }
        public string Message { get; set; }
    }
}

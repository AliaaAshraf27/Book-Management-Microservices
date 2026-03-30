using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class BookReturnedEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal LateFees { get; set; } 
        public string Message { get; set; }
    }
}

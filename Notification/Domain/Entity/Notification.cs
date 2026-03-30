using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public Guid? BorrowingId { get; set; }   
        public string? BookTitle { get; set; }   
        public string? UserName { get; set; }    

    }
}

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Notifications.DTOS
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Created;
        public Guid? BorrowingId { get; set; }
        public string? BookTitle { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

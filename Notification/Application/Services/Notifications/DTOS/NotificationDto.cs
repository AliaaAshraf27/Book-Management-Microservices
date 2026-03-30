using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Notifications.DTOS
{
    public class NotificationDto
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}

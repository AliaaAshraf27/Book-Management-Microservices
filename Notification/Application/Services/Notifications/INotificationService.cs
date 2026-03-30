using Application.Response;
using Application.Services.Notifications.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Notifications
{
    public interface INotificationService
    {
        Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto);
        Task<ApiResponse<List<NotificationDto>>> GetAllAsync();
        Task<ApiResponse<List<NotificationDto>>> GetByUserIdAsync(string userId, bool? isRead);
        Task<ApiResponse<bool>> MarkAsReadAsync(Guid id, string userId);
    }
}

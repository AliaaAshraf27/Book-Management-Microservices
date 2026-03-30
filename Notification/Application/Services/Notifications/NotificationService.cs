using Application.IRepository;
using Application.Response;
using Application.Services.Notifications.DTOS;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Notifications
{
    public class NotificationService(INotificationRepository _repository, IMapper _mapper ,ISender _sender) : INotificationService
    {
        public async Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = dto.UserId,
                    UserName = dto.UserName,
                    Message = dto.Message,
                    Type = dto.Type,
                    Status = dto.Status,
                    BorrowingId = dto.BorrowingId,
                    BookTitle = dto.BookTitle,
                    CreatedAt = dto.CreatedAt ?? DateTime.UtcNow
                };

                await _repository.AddAsync(notification);
                var result = _mapper.Map<NotificationDto>(notification);
                await _sender.SendAsync(dto.UserId, result);

                return ApiResponse<NotificationDto>.SuccessResponse(result, "Notification created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<NotificationDto>.FailureResponse(ex.Message);
            }
        }
        public async Task<ApiResponse<List<NotificationDto>>> GetAllAsync()
        {
            var notifications = await _repository.GetAllAsync();
            if (notifications == null || !notifications.Any())
                return ApiResponse<List<NotificationDto>>.FailureResponse("No notifications found");
            var data = _mapper.Map<List<NotificationDto>>(notifications);
            return ApiResponse<List<NotificationDto>>.SuccessResponse(data , null);
        }
        public async Task<ApiResponse<List<NotificationDto>>> GetByUserIdAsync(string userId, bool? isRead)
        {
            var notifications = await _repository.GetByFilterAsync(userId, isRead);
            if (notifications == null || !notifications.Any())
                return ApiResponse<List<NotificationDto>>.FailureResponse("No notifications found for this user");

            var data = _mapper.Map<List<NotificationDto>>(notifications);
            return ApiResponse<List<NotificationDto>>.SuccessResponse(data , null);
        }
        public async Task<ApiResponse<bool>> MarkAsReadAsync(Guid id, string userId)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification == null)
                return ApiResponse<bool>.FailureResponse("Notification not found");

            if (notification.UserId != userId)
                return ApiResponse<bool>.FailureResponse("You are not authorized to update this notification");

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _repository.UpdateAsync(notification);
            }
            return ApiResponse<bool>.SuccessResponse(true, "Notification marked as read");
        }
    }
}

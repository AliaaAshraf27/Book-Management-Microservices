using Application.Events;
using Application.IRepository;
using Application.Services.Consumer.DTOS;
using Application.Services.Notifications;
using Application.Services.Notifications.DTOS;
using Domain.Entity;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UserRegisteredHandle(INotificationService _repo, IHttpClientFactory _httpClientFactory)
    {
        public async Task Handle(UserRegisteredEvent userEvent)
        {
            await _repo.CreateAsync(new CreateNotificationDto
            {
                UserId = userEvent.UserId,
                Type = NotificationType.UserRegistered,
                Message = $"Welcome {userEvent.UserName}!"
            });

            var client = _httpClientFactory.CreateClient("AccountApi");
            var admins = await client.GetFromJsonAsync<List<AdminDto>>("Admin") ?? new();

            foreach (var admin in admins)
            {
                await _repo.CreateAsync(new CreateNotificationDto
                {
                    UserId = admin.Id,
                    Type = NotificationType.UserRegistered,
                    Message = $"New user registered: {userEvent.UserName} notify to {admin.UserName}"
                });
            }
        }
    }
}

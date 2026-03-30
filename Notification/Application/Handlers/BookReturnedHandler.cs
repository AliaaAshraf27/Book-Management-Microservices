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
    public class BookReturnedHandler(INotificationService _repo , IHttpClientFactory _httpClientFactory)
    {
        public async Task Handle(BookReturnedEvent returnedEvent)
        {
            var client = _httpClientFactory.CreateClient("AccountApi");
            var admins = await client.GetFromJsonAsync<List<AdminDto>>("Admin") ?? new();

            foreach (var admin in admins)
            {
                await _repo.CreateAsync(new CreateNotificationDto
                {
                    UserName = returnedEvent.UserName,
                    UserId = returnedEvent.UserId,
                    BookTitle = returnedEvent.BookTitle,
                    Type = NotificationType.BookReturned,
                    Status = NotificationStatus.Created,
                    Message =
                    $"User {returnedEvent.UserName} returned '{returnedEvent.BookTitle}'. Late fees: {returnedEvent.LateFees} EGP"
                });
            }

        }
    
    }
}

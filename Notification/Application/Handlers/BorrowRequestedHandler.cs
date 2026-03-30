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
    public class BorrowRequestedHandler(INotificationService _repo, IHttpClientFactory _httpClientFactory)
    {
        public async Task Handle(BorrowRequestedEvent borrowEvent)
        {
            var client = _httpClientFactory.CreateClient("AccountApi");
            var admins = await client.GetFromJsonAsync<List<AdminDto>>("Admin") ?? new();

            foreach (var admin in admins)
            {
                await _repo.CreateAsync(new CreateNotificationDto
                {
                    UserName = borrowEvent.UserName,
                    UserId = borrowEvent.UserId,
                    Type = NotificationType.BorrowRequested,
                    Message = $"New borrow request: {borrowEvent.UserName} wants '{borrowEvent.BookTitle}'",
                    BorrowingId = borrowEvent.BorrowingId,
                    BookTitle = borrowEvent.BookTitle,
                    CreatedAt = borrowEvent.RequestDate
                });
            };
        }
    }
}

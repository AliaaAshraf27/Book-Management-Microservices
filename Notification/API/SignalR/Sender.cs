using Application.IRepository;
using Application.Services.Notifications.DTOS;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class Sender(IHubContext<NotificationHub> _hub) : ISender
    {
        public async Task SendAsync(string userId, NotificationDto notification)
        {
            await _hub.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}

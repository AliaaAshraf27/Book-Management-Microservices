using Application.Services.Notifications.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface ISender
    {
        Task SendAsync(string userId, NotificationDto notification);
    }
}

using Domain.Entity;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task<List<Notification>> GetByUserIdAsync(string userId);
        Task<List<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(Guid id);
        Task<List<Notification>> GetByFilterAsync(string userId, bool? IsRead);

    }
}

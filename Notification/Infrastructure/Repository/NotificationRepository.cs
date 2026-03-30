using Application.IRepository;
using Domain.Entity;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class NotificationRepository(ApplicationDbContext _context) : INotificationRepository
    {
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            _context.SaveChanges();
        }
        public async Task UpdateAsync(Notification notification)
        {
             _context.Notifications.Update(notification);
            _context.SaveChanges();
        }
        public async Task<List<Notification>> GetByUserIdAsync(string userId)
            => await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        public async Task<List<Notification>> GetAllAsync() => await _context.Notifications.ToListAsync();
        public async Task<Notification?> GetByIdAsync(Guid id) =>
            await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        public async Task<List<Notification>> GetByFilterAsync(string userId , bool? IsRead) 
            => await _context.Notifications.Where(n => n.UserId == userId &&
                   (!IsRead.HasValue || n.IsRead == IsRead.Value))
            .OrderByDescending(n => n.CreatedAt).AsNoTracking().ToListAsync();
        
    }
}

using Application.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class FineRepository(ApplicationDbContext _context) : IFineRepository
    {
        public async Task<List<Fine>> GetAll() =>
            await _context.Fines.Include(u => u.User).Include(b => b.Borrowing)
            .ThenInclude(b => b.Book).ToListAsync();
        public async Task<Fine?> GetById(Guid Id) =>
            await _context.Fines.Include(u => u.User).Include(b => b.Borrowing).ThenInclude(b => b.Book)
            .SingleOrDefaultAsync(b => b.Id == Id);
        public async Task<List<Fine>> GetByUserId(string userId) =>
            await _context.Fines.Include(u => u.User).Include(b => b.Borrowing).ThenInclude(b => b.Book)
            .Where(b => b.UserId == userId).ToListAsync();
        public async Task AddAsync(Fine fine)
        {
            await _context.Fines.AddAsync(fine);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Fine fine)
        {
            _context.Fines.Update(fine);
            await _context.SaveChangesAsync();
        }
    }
}

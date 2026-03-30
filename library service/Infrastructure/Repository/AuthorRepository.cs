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
    public class AuthorRepository(ApplicationDbContext _context) :IAuthorRepository
    {
        public async Task<List<Author>> GetAllAsync() => await _context.Authors.AsNoTracking().ToListAsync();
        public async Task<Author?> GetByIdAsync(Guid Id) => await _context.Authors
            .Include(b => b.Books).ThenInclude(b => b.Category).AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid Id)
        {
            var author = await _context.Authors.FindAsync(Id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

    }
}

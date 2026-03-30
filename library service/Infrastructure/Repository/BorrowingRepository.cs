using Application.DTOs;
using Application.DTOs.Book;
using Application.DTOs.Borrowing;
using Application.Enums;
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
    public class BorrowingRepository(ApplicationDbContext _context) : IBorrowingRepository
    {
        public async Task<List<Borrowing>> GetAllAsync() => await _context.Borrowings
          .Include(b => b.Book).AsNoTracking().ToListAsync();
        public async Task<List<PopularBookDto>> GetBorrowedBooksAsync() => 
            await _context.Borrowings.Where(b => b.Status == BorrowingStatus.Approved)
            .GroupBy(b => b.BookId).Select(g => new PopularBookDto
            {
                BookId = g.Key,
                BookTitle = g.First().Book.Title,
                //image = Convert.ToBase64String(g.First().Book.Image),
                image = g.First().Book.Image != null
                        ? Convert.ToBase64String(g.First().Book.Image)
                        : null,
                BorrowingCount = g.Count()

            }).OrderByDescending(x => x.BorrowingCount).AsNoTracking().ToListAsync();
        public async Task<Borrowing?> GetByIdAsync(Guid borrowingId) => await _context.Borrowings
                 .Include(b => b.Book).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == borrowingId);
        public async Task<List<Borrowing>> GetByUserIdAsync(string userId)
            => await _context.Borrowings.Include(l => l.Book)
             .Where(l => l.UserId == userId).ToListAsync();
        public async Task<PagedResult<Borrowing>> GetPagedAsync(BorrowingFilterDto dto)
        {
            var query = _context.Borrowings.Include(b => b.Book).AsQueryable();
            var totalCount = await query.CountAsync();
            var borrowings = await query
                .Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync();

            return new PagedResult<Borrowing>
            {
                Items = borrowings,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Borrowing loan)
        {
            _context.Borrowings.Update(loan);
             await _context.SaveChangesAsync();
        }
        public async Task<bool> HasActiveBorrowingAsync(string userId, Guid bookId) 
            => await _context.Borrowings.AnyAsync(b =>
                b.UserId == userId && b.BookId == bookId &&
                (b.Status == BorrowingStatus.Pending || b.Status == BorrowingStatus.Approved));
      
    }
}

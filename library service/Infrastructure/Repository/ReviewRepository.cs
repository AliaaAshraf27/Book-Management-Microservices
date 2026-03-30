using Application.DTOs;
using Application.DTOs.Review;
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
    public class ReviewRepository(ApplicationDbContext _context) : IReviewRepository
    {
        public async Task<PagedResult<Review>> GetPagedAsync(ReviewFilterDto dto)
        {
            var query = ApplyFilter(dto);

            var totalCount = await query.CountAsync();
            var reviews = await query
                .Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync();

            return new PagedResult<Review>
            {
                Items = reviews,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<Review?> GetByIdAsync(Guid Id) => await _context.Reviews
           .Include(c => c.Book).Include(u => u.User).FirstOrDefaultAsync(b => b.Id == Id);
        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid Id)
        {
            var review = await _context.Reviews.FindAsync(Id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        #region Private method
        private IQueryable<Review> ApplyFilter(ReviewFilterDto model)
        {
            var query = _context.Reviews.Include(b => b.Book).Include(u => u.User).AsQueryable();
            if (!string.IsNullOrEmpty(model.BookTitle))
                query.Where(b => b.Book.Title.Contains(model.BookTitle));

            if (!string.IsNullOrEmpty(model.UserName))
                query.Where(b => b.User.Name.Contains(model.UserName));

            if (!string.IsNullOrEmpty(model.Text))
                query.Where(b => b.Text.Contains(model.Text));

            if (model.Rating.HasValue)
                query.Where(b => b.Rating == model.Rating);

            if (model.CreatedAt.HasValue)
                query.Where(b => b.CreatedAt == model.CreatedAt);

            return query.OrderBy(b => b.CreatedAt);

        }
        #endregion
    }
}

using Application.DTOs;
using Application.DTOs.Book;
using Application.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Reflection.Metadata.BlobBuilder;

namespace Infrastructure.Repository
{
    public class BookRepository(ApplicationDbContext _context) : IBookRepository
    {
        public async Task<List<Book>> GetAllAsync() => 
            await _context.Books.Include(c => c.Category).ToListAsync();
        public async Task<PagedResult<Book>> GetPagedAsync(BookFilterDto dto)
        {
            var query = ApplyFilter(dto);

            var totalCount = await query.CountAsync();
            var books = await query
                .Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync();

            return new PagedResult<Book>
            {
                Items = books,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task<Book?> GetByIdAsync(Guid Id) => await _context.Books
            .Include(c => c.Category).Include(a => a.Author).FirstOrDefaultAsync(b => b.Id == Id);
        public async Task<List<Book>> GetByCategoryIdAsync(Guid categoryId) =>
            await _context.Books.Include(c => c.Category)
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid Id)
        {
           var book = await _context.Books.FindAsync(Id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        public IQueryable<Book> ApplyFilter(BookFilterDto model)
        {
            var query = _context.Books.Include(b => b.Category).Include(a => a.Author).AsQueryable();
            if (!string.IsNullOrEmpty(model.Title))
                query = query.Where(b => b.Title.Contains(model.Title));

            if (!string.IsNullOrEmpty(model.AuthorName))
                query = query.Where(b => b.Author.Name.Contains(model.AuthorName));

            if (!string.IsNullOrEmpty(model.Category))
                query = query.Where(b => b.Category.Name.Contains(model.Category));

            if (model.PublishedYear.HasValue)
                query = query.Where(b => b.PublishedYear == model.PublishedYear);

            if (model.MinPrice.HasValue)
                query = query.Where(b => b.Price >= model.MinPrice);

            if (model.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= model.MaxPrice);

            return query.OrderBy(b => b.Title);

        }

    }
}

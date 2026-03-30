using Application.DTOs;
using Application.DTOs.Book;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid Id);
        Task<List<Book>> GetByCategoryIdAsync(Guid categoryId);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid Id);
        Task<PagedResult<Book>> GetPagedAsync(BookFilterDto dto);
        IQueryable<Book> ApplyFilter(BookFilterDto model);


    }
}

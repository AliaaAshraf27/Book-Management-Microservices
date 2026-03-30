using Application.DTOs;
using Application.DTOs.Book;
using Application.DTOs.Borrowing;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IBorrowingRepository
    {
        Task<List<Borrowing>> GetAllAsync();
        Task<Borrowing?> GetByIdAsync(Guid BookId);
        Task<List<Borrowing>> GetByUserIdAsync(string userId);
        Task<PagedResult<Borrowing>> GetPagedAsync(BorrowingFilterDto dto);
        Task<List<PopularBookDto>> GetBorrowedBooksAsync();
        Task AddAsync(Borrowing loan);
        Task UpdateAsync(Borrowing loan);
        Task<bool> HasActiveBorrowingAsync(string userId, Guid bookId);
    }
}

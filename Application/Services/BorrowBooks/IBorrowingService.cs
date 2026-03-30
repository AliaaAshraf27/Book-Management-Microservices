using Application.DTOs;
using Application.DTOs.Borrowing;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BorrowBooks
{
    public interface IBorrowingService
    {
        Task<ApiResponse<List<BorrowingDto>>> GetAllAsync();
        Task<ApiResponse<PagedResult<BorrowingDto>>> GetPagedAsync(BorrowingFilterDto dto);
        Task<ApiResponse<List<UserBorrowingDto>>> GetUserBorrowingsAsync(string userId);
        Task<ApiResponse<bool>> BorrowBookAsync(BorrowBookDto dto, string userId);
        Task<ApiResponse<bool>> ApproveBorrowingAsync(Guid borrowingId);
        Task<ApiResponse<bool>> ReturnBookAsync(Guid borrowingId);
    }
}

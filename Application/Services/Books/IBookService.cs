using Application.DTOs;
using Application.DTOs.Book;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Books
{
    public interface IBookService
    {
        Task<ApiResponse<List<BooksDTO>>> GetAllAsync();
        Task<ApiResponse<PagedResult<BooksDTO>>> GetPagedAsync(BookFilterDto filterDto);
        Task<ApiResponse<BookDetailsDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<BooksDTO>>> GetByCategoryIdAsync(Guid categoryId);
        Task<ApiResponse<List<BooksDTO>>> SearchAsync(BookFilterDto dto);
        Task<ApiResponse<List<PopularBookDto>>> PopularAsync();
        Task<ApiResponse<bool>> CreateAsync(CreateBookDto model);
        Task<ApiResponse<bool>> UpdateAsync(UpdateBookDto model);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}

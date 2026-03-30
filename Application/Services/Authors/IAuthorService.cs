using Application.DTOs.Author;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authors
{
    public interface IAuthorService
    {
        Task<ApiResponse<List<AuthorsDTO>>> GetAllAsync();
        Task<ApiResponse<AuthorDetailsDTO>> GetByIdAsync(Guid id);
        Task<ApiResponse<GetAuthorWithBooks>> GetByIdWithBooksAsync(Guid id);
        Task<ApiResponse<bool>> CreateAsync(CreateAuthorDTO model);
        Task<ApiResponse<bool>> UpdateAsync(UpdateAuthorDTO model);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}

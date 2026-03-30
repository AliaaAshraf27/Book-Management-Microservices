using Application.DTOs.Category;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Categories
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDTO>>> GetAllAsync();
        Task<ApiResponse<bool>> CreateAsync(CategoryDTO dto);
        Task<ApiResponse<bool>> UpdateAsync(UpdateCategoryDto dto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}

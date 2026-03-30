using Application.DTOs.Category;
using Application.IRepository;
using Application.Response;
using Application.Services.Books;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Categories
{
    public class CategoryService(ICategoryRepository _categoryRepo, IStringLocalizer<CategoryService> _localizer, IMapper _mapper) : ICategoryService
    {
        public async Task<ApiResponse<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            if (categories == null || !categories.Any())
                return ApiResponse<List<CategoryDTO>>.FailureResponse(_localizer["CategoryNotFound"]);

            var data = _mapper.Map<List<CategoryDTO>>(categories);
            return ApiResponse<List<CategoryDTO>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<bool>> CreateAsync(CategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };
            await _categoryRepo.AddAsync(category);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["CategoryCreated"]);
        }
        public async Task<ApiResponse<bool>> UpdateAsync(UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(dto.Id);
            if (existingCategory == null)
                return ApiResponse<bool>.FailureResponse(_localizer["CategoryNotFound"]);

            existingCategory.Name = dto.Name;
            await _categoryRepo.UpdateAsync(existingCategory);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["CategoryUpdated"]);
        }
        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null)
                return ApiResponse<bool>.FailureResponse(_localizer["CategoryNotFound"]);

            await _categoryRepo.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["CategoryDeleted"]);
        }
    }
}

using Application.DTOs.Category;
using Application.Services.Books;
using Application.Services.Categories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>Ok(await _categoryService.GetAllAsync());

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _categoryService.CreateAsync(dto));
        }
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _categoryService.UpdateAsync(dto));
        }
        [Authorize(policy: "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Id) =>
           Ok(await _categoryService.DeleteAsync(Id));
    }
}

using Application.DTOs.Author;
using Application.Services.Authors;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICore.Controllers
{
    [Route("api/Author")]
    [ApiController]
    [Authorize]
    public class AuthorController(IAuthorService _authorService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()=> Ok( await _authorService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid Id) => Ok(await _authorService.GetByIdAsync(Id));

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetByIdWithBooks(Guid Id) =>
            Ok( await _authorService.GetByIdWithBooksAsync(Id));

        [Authorize(policy: "Admin")]
        [HttpPost] 
        public async Task<IActionResult> Create([FromForm] CreateAuthorDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authorService.CreateAsync(model));
        }

        [Authorize(policy: "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateAuthorDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authorService.UpdateAsync(model));
        }

        [Authorize(policy: "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Id) => Ok(await _authorService.DeleteAsync(Id));

    }
}

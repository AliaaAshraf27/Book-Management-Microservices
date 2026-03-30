using Application.DTOs.Book;
using Application.Services.Books;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace APICore.Controllers
{

    [Route("api/Book")]
    [ApiController]
    [Authorize]
    public class BookController(IBookService _bookService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok( await _bookService.GetAllAsync());

        [HttpGet("Paged")]
        public async Task<IActionResult> GetPaged([FromQuery] BookFilterDto filterDto) =>
            Ok(await _bookService.GetPagedAsync(filterDto));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid Id) => Ok(await _bookService.GetByIdAsync(Id));

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId) 
            => Ok( await _bookService.GetByCategoryIdAsync(categoryId));

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] BookFilterDto filterDto) =>
            Ok(await _bookService.SearchAsync(filterDto));

        [HttpGet("Popular")]
        public async Task<IActionResult> Popular() => Ok(await _bookService.PopularAsync());

        [Authorize(policy:"Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateBookDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _bookService.CreateAsync(model));
        }

        [Authorize(policy: "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBookDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
       
            return Ok(await _bookService.UpdateAsync(model));
        }

        [Authorize(policy: "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (Guid id) => Ok( await _bookService.DeleteAsync(id));

    }
}

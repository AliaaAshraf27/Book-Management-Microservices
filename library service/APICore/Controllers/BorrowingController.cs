using Application.DTOs.Borrowing;
using Application.Services.BorrowBooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APICore.Controllers
{
    [Route("api/Borrowing")]
    [ApiController]
    public class BorrowingController(IBorrowingService _borrowingService) : ControllerBase
    {
        [Authorize(policy: "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _borrowingService.GetAllAsync());

        [Authorize(policy: "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserBorrowing()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _borrowingService.GetUserBorrowingsAsync(userId));
        }

        [Authorize(policy: "Admin")]
        [HttpGet("Paged")]
        public async Task<IActionResult> GetPagedBorrowings([FromQuery] BorrowingFilterDto dto) =>
            Ok(await _borrowingService.GetPagedAsync(dto));

        [Authorize(policy: "User")]
        [HttpPost("Borrow")]
        public async Task<IActionResult> BorrowBook( BorrowBookDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _borrowingService.BorrowBookAsync(dto , userId));
        }

        [Authorize(policy: "Admin")]
        [HttpPost("Approve")]
        public async Task<IActionResult> ApproveBook(Guid borrowingId) =>
            Ok(await _borrowingService.ApproveBorrowingAsync(borrowingId));

        [Authorize(policy: "User")]
        [HttpPost("Return")]
        public async Task<IActionResult> ReturnBook(Guid borrowingId) => 
            Ok(await _borrowingService.ReturnBookAsync(borrowingId));


    }
}

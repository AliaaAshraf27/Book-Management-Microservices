using Application.DTOs.Review;
using Application.Services.BorrowBooks;
using Application.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APICore.Controllers
{
    [Route("api/Review")]
    [ApiController]
    public class ReviewController(IReviewService _reviewService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPagedReviews([FromQuery] ReviewFilterDto dto) =>
           Ok(await _reviewService.GetPagedReviewAsync(dto));

        [Authorize(policy: "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _reviewService.AddReviewAsync(model, userId));
        }

        [Authorize(policy: "User")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateReviewDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _reviewService.UpdateReviewAsync(model));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _reviewService.DeleteReviewAsync(id));

    }
}

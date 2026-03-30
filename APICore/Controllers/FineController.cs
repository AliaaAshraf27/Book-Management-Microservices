using Application.DTOs.Fine;
using Application.Services.Books;
using Application.Services.Fine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APICore.Controllers
{
    [Route("api/Fine")]
    [ApiController]
    public class FineController(IFineService _fineService) : ControllerBase
    {
        [Authorize(policy: "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _fineService.GetAllAsync());

        [Authorize(policy: "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid Id) => Ok(await _fineService.GetByIdAsync(Id));

        [Authorize(policy: "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserFines()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            return Ok( await _fineService.GetUserFinesAsync(userId));
        }

        [Authorize(policy: "User")]
        [HttpPost("PayFine")]
        public async Task<IActionResult> PayFine(PayFineDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _fineService.PayFineAsync(model , userId));
        }
    }
}

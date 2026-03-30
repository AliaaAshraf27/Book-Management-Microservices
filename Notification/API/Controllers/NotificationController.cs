using Application.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/Notification")]
    [ApiController]
    public class NotificationController(INotificationService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetAllByUserId(bool? isRead)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token - user id not found");

            var result = await _service.GetByUserIdAsync(userId , isRead);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("mark-read")]
        public async Task<IActionResult> MarkAsRead(Guid Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _service.MarkAsReadAsync(Id, userId);
            return Ok(new { message = "Notification marked as read successfully" });
        }
    }
}

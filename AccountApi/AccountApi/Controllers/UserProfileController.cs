using Application.DTOs;
using Application.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ClientModel.Primitives;
using System.Security.Claims;

namespace AccountApi.Controllers
{
    [Route("api/Profile")]
    [ApiController]
    [Authorize]
    public class UserProfileController(IUserProfileService _userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            return Ok(await _userService.GetCurrentUserProfileAsync(userId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");
            
            return Ok(await _userService.UpdateProfileAsync(model , userId));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            return Ok(await _userService.DeleteProfileAsync(userId));
        }
    }
}

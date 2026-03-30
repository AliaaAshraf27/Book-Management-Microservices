using Application.DTOs;
using Application.IRepository;
using Application.Services.Auth;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountApi.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO model) // defualt FromBody
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authService.LoginAsync(model));
        }

        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is required."); 
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(result.Data.Token))
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _authService.LogoutAsync(userId));
        }

        [Authorize]
        [HttpPost("Change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _authService.ChangePasswordAsync(model, userId));
        }

        [HttpPost("Forget-password")]
        public async Task<IActionResult> ForgetPassword(string Email) =>
            Ok(await _authService.ForgetPasswordAsync(Email));

        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model) =>
           Ok(await _authService.ResetPasswordAsync(model));
    }
}

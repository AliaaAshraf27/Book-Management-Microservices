using Application.Services.Auth.DTOs;
using Application.Services.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountApi.Controllers
{
    [Route("api/Register")]
    [ApiController]
    public class RegisterController(IRegisterService _registerService) : ControllerBase
    {
        [HttpPost] 
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _registerService.RegisterAsync(model));
        }
        [HttpPost("Admin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _registerService.RegisterAdminAsync(model));
        }

    }
}

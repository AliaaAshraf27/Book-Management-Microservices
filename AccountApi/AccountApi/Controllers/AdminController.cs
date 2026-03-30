using Application.Services.Auth;
using Application.Services.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountApi.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController(IAdminService _adminService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAdmins() => Ok( await _adminService.GetAdminsAsync());
    }
}
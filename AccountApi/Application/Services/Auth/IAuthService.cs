using Application.DTOs;
using Application.Response;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto);
        Task<ApiResponse<AuthResponseDTO>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> LogoutAsync(string userId);
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, string userId);
        Task<ApiResponse<string>> ForgetPasswordAsync(string email);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto);
    }
}

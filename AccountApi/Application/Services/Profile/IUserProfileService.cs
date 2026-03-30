using Application.DTOs;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Profile
{
    public interface IUserProfileService
    {
        Task<ApiResponse<UserProfileDTO>> GetCurrentUserProfileAsync(string userId);
        Task<ApiResponse<bool>> UpdateProfileAsync(UpdateProfileDto model, string userId);
        Task<ApiResponse<bool>> DeleteProfileAsync(string userId);
    }
}

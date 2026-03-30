using Application.DTOs;
using Application.Response;
using Application.Services.Register;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Profile
{
    public class AdminService(UserManager<User> _userManager, IStringLocalizer<AdminService> _localizer, IMapper _mapper) : IAdminService
    {
        public async Task<ApiResponse<List<UsersDto>>> GetAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins == null || !admins.Any())
                return ApiResponse<List<UsersDto>>.FailureResponse(_localizer["NotFoundAnyAdmins"]);

            var data = _mapper.Map<List<UsersDto>>(admins);
            return ApiResponse<List<UsersDto>>.SuccessResponse(data, null);
        }
    }
}

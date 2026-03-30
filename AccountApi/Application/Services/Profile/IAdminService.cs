using Application.DTOs;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Profile
{
    public interface IAdminService
    {
        Task<ApiResponse<List<UsersDto>>> GetAdminsAsync();
    }
}

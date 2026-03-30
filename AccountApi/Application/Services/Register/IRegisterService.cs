using Application.Response;
using Application.Services.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Register
{
    public interface IRegisterService
    {
        Task<ApiResponse<bool>> RegisterAsync(RegisterModel model);
        Task<ApiResponse<bool>> RegisterAdminAsync(RegisterModel model);
    }
}

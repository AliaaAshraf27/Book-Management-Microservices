using Application.DTOs.Fine;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Fine
{
    public interface IFineService
    {
        Task<ApiResponse<List<FineDto>>> GetAllAsync();
        Task<ApiResponse<FineDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<UserFineDto>>> GetUserFinesAsync(string userId);
        Task<ApiResponse<bool>> PayFineAsync(PayFineDto dto, string userId);
    }
}

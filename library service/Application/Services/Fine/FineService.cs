using Application.DTOs.Fine;
using Application.IRepository;
using Application.Response;
using Application.Services.Books;
using AutoMapper;
using Domain.Enums;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Fine
{
    public class FineService(IFineRepository _fineRepo, IMapper _mapper, IStringLocalizer<FineService> _localizer) : IFineService
    {
        public async Task<ApiResponse<List<FineDto>>> GetAllAsync()
        {
            var fines = await _fineRepo.GetAll();
            if (fines == null || !fines.Any())
                return ApiResponse<List<FineDto>>.FailureResponse(_localizer["NotFoundAnyFine"]);

            var data = _mapper.Map<List<FineDto>>(fines);
            return ApiResponse<List<FineDto>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<FineDto>> GetByIdAsync(Guid id)
        {
            var fine = await _fineRepo.GetById(id);
            if (fine == null)
                return ApiResponse<FineDto>.FailureResponse(_localizer["FineNotFound"]);

            var data = _mapper.Map<FineDto>(fine);
            return ApiResponse<FineDto>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<List<UserFineDto>>> GetUserFinesAsync(string userId)
        {
            var fines = await _fineRepo.GetByUserId(userId);
            if (fines == null || !fines.Any())
                return ApiResponse<List<UserFineDto>>.FailureResponse(_localizer["NotFoundAnyFine"]);

            var data = _mapper.Map<List<UserFineDto>>(fines);
            return ApiResponse<List<UserFineDto>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<bool>> PayFineAsync(PayFineDto dto, string userId)
        {
            var fine = await _fineRepo.GetById(dto.FineId);
            if (fine == null) return ApiResponse<bool>.FailureResponse(_localizer["FineNotFound"]);

            if (fine.UserId != userId)
                return ApiResponse<bool>.FailureResponse(_localizer["UnauthorizedFinePayment"]);

            if (fine.PaidStatus == PaidStatus.Paid)
                return ApiResponse<bool>.FailureResponse(_localizer["FineAlreadyPaid"]);

            if (dto.Amount < fine.FineAmount)
                return ApiResponse<bool>.FailureResponse(_localizer["FullPaymentRequired"]);

            fine.FineAmount = 0;
            fine.PaidStatus = PaidStatus.Paid;
            fine.PaymentDate = DateTime.UtcNow;
            fine.UpdatedAt = DateTime.UtcNow;
            await _fineRepo.UpdateAsync(fine);

            return ApiResponse<bool>.SuccessResponse(true, _localizer["FinePaymentSuccessful"]);
        }

    }
}

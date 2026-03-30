using Application.DTOs;
using Application.DTOs.Review;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Reviews
{
    public interface IReviewService
    {
        Task<ApiResponse<PagedResult<ReviewsDto>>> GetPagedReviewAsync(ReviewFilterDto dto);
        Task<ApiResponse<bool>> AddReviewAsync(CreateReviewDto model, string userId);
        Task<ApiResponse<bool>> UpdateReviewAsync(UpdateReviewDto model);
        Task<ApiResponse<bool>> DeleteReviewAsync(Guid id);

    }
}

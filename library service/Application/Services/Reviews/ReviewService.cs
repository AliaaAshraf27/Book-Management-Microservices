using Application.DTOs;
using Application.DTOs.Book;
using Application.DTOs.Review;
using Application.IRepository;
using Application.Response;
using Application.Services.BorrowBooks;
using Application.Services.RabbitMQ;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Reviews
{
    public class ReviewService(IReviewRepository _reviewRepo, IBookRepository _bookRepo, IMapper _mapper, IStringLocalizer<ReviewService> _localizer, IRabbitMQPublisher _publish) : IReviewService
    {
        public async Task<ApiResponse<PagedResult<ReviewsDto>>> GetPagedReviewAsync(ReviewFilterDto dto)
        {
            var reviews = await _reviewRepo.GetPagedAsync(dto);
            if (reviews == null || reviews.Items == null || !reviews.Items.Any())
                return ApiResponse<PagedResult<ReviewsDto>>.FailureResponse(_localizer["NotFoundAnyReview"]);

            var list = _mapper.Map<List<ReviewsDto>>(reviews.Items);
            var paged = new PagedResult<ReviewsDto>
            {
                Items = list,
                PageNumber = reviews.PageNumber,
                PageSize = reviews.PageSize,
                TotalCount = reviews.TotalCount
            };
            return ApiResponse<PagedResult<ReviewsDto>>.SuccessResponse(paged, null);
        }
        public async Task<ApiResponse<bool>> AddReviewAsync(CreateReviewDto model, string userId)
        {
            var book = await _bookRepo.GetByIdAsync(model.bookId);
            if (book == null)
                return ApiResponse<bool>.FailureResponse(_localizer["BookNotFound"]);

            var review = _mapper.Map<Review>(model);
            review.UserId = userId;
            await _reviewRepo.AddAsync(review);

            return ApiResponse<bool>.SuccessResponse(true, _localizer["ReviewAdded"]);
        }
        public async Task<ApiResponse<bool>> UpdateReviewAsync(UpdateReviewDto model)
        {
            if (model.bookId.HasValue)
            {
                var book = await _bookRepo.GetByIdAsync(model.bookId.Value);
                if (book == null)
                    return ApiResponse<bool>.FailureResponse(_localizer["BookNotFound"]);
            }
            var existReview = await _reviewRepo.GetByIdAsync(model.ReviewId);
            if (existReview == null)
                return ApiResponse<bool>.FailureResponse(_localizer["ReviewNotFound"]);

            _mapper.Map(model, existReview);
            await _reviewRepo.UpdateAsync(existReview);

            return ApiResponse<bool>.SuccessResponse(true, _localizer["ReviewUpdated"]);
        }
        public async Task<ApiResponse<bool>> DeleteReviewAsync(Guid id)
        {
            var existReview = await _reviewRepo.GetByIdAsync(id);
            if (existReview == null)
                return ApiResponse<bool>.FailureResponse(_localizer["ReviewNotFound"]);

            await _reviewRepo.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["ReviewDeleted"]);
        }

    }
}

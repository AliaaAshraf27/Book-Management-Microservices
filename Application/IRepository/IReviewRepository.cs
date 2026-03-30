using Application.DTOs;
using Application.DTOs.Review;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IReviewRepository
    {
        Task<PagedResult<Review>> GetPagedAsync(ReviewFilterDto dto);
        Task<Review?> GetByIdAsync(Guid Id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Guid Id);
    }
}

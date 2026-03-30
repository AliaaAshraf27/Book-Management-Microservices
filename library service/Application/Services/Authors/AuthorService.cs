using Application.DTOs.Author;
using Application.IRepository;
using Application.Response;
using Application.Services.Categories;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authors
{
    public class AuthorService(IAuthorRepository _authorRepository, IMapper _mapper, IStringLocalizer<AuthorService> _localizer) : IAuthorService
    {
        public async Task<ApiResponse<List<AuthorsDTO>>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            if (authors == null || !authors.Any())
                return ApiResponse<List<AuthorsDTO>>.FailureResponse(_localizer["NoAuthorsFound"]);

            var data = _mapper.Map<List<AuthorsDTO>>(authors);
            return ApiResponse<List<AuthorsDTO>>.SuccessResponse(data , null);
        }
        public async Task<ApiResponse<AuthorDetailsDTO>> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                return ApiResponse<AuthorDetailsDTO>.FailureResponse(_localizer["AuthorNotFound"]);

            var data = _mapper.Map<AuthorDetailsDTO>(author);
            return ApiResponse<AuthorDetailsDTO>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<GetAuthorWithBooks>> GetByIdWithBooksAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                return ApiResponse<GetAuthorWithBooks>.FailureResponse(_localizer["AuthorNotFound"]);

            var data = _mapper.Map<GetAuthorWithBooks>(author);
            return ApiResponse<GetAuthorWithBooks>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<bool>> CreateAsync(CreateAuthorDTO model)
        {
            if (!ValidateDates(model.DateOfBirth, model.DateOfDeath))
                return ApiResponse<bool>.FailureResponse(_localizer["InvalidDates"]);

            var map = _mapper.Map<Author>(model);
            await _authorRepository.AddAsync(map);

            return ApiResponse<bool>.SuccessResponse(true, _localizer["AuthorAdded"]);
        }
        public async Task<ApiResponse<bool>> UpdateAsync(UpdateAuthorDTO model)
        {
           var existAuthor = await _authorRepository.GetByIdAsync(model.Id);
            if (existAuthor == null)
                return ApiResponse<bool>.FailureResponse( _localizer["AuthorNotFound"]);

            if (!ValidateDates(model.DateOfBirth, model.DateOfDeath))
                return ApiResponse<bool>.FailureResponse(_localizer["InvalidDates"]);

            _mapper.Map(model, existAuthor);
            await _authorRepository.UpdateAsync(existAuthor); 
            return ApiResponse<bool>.SuccessResponse(true, _localizer["AuthorUpdated"]);
        }
        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var existAuthor = await _authorRepository.GetByIdAsync(id);
            if (existAuthor == null)
                return ApiResponse<bool>.FailureResponse(_localizer["AuthorNotFound"] );

            await _authorRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["AuthorDeleted"]);
        }

        #region Private method
        private bool ValidateDates(DateOnly? dateOfBirth, DateOnly? dateOfDeath)
        {
            if (dateOfBirth.HasValue && dateOfDeath.HasValue)
                return dateOfDeath >= dateOfBirth;

            return true;
        }
        #endregion
    }
}

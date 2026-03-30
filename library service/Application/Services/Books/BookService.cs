using Application.DTOs;
using Application.DTOs.Book;
using Application.IRepository;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Books
{
    public class BookService(IBookRepository _bookRepo, IBorrowingRepository _borrowingRepo, ICategoryRepository _categoryRepo, IAuthorRepository _authorRepo, IMapper _mapper, IStringLocalizer<BookService> _localizer) : IBookService
    {
        public async Task<ApiResponse<List<BooksDTO>>> GetAllAsync()
        {
            var books = await _bookRepo.GetAllAsync();
            if (books == null || !books.Any())
                return ApiResponse<List<BooksDTO>>.FailureResponse(_localizer["NotFoundAnyBooks"]);

            var data = _mapper.Map<List<BooksDTO>>(books);
            return ApiResponse<List<BooksDTO>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<PagedResult<BooksDTO>>> GetPagedAsync(BookFilterDto filterDto)
        {
            var result = await _bookRepo.GetPagedAsync(filterDto);
            if (result == null || result.Items == null || !result.Items.Any())
                return ApiResponse<PagedResult<BooksDTO>>.FailureResponse(_localizer["NotFoundAnyBooks"]);

            var dto = _mapper.Map<List<BooksDTO>>(result.Items);
            var paged = new PagedResult<BooksDTO>
            {
                Items = dto,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
            return ApiResponse<PagedResult<BooksDTO>>.SuccessResponse(paged, null);
        }
        public async Task<ApiResponse<BookDetailsDto>> GetByIdAsync(Guid id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
                return ApiResponse<BookDetailsDto>.FailureResponse( _localizer["BookNotFound"]);

            var data = _mapper.Map<BookDetailsDto>(book);
            return ApiResponse<BookDetailsDto>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<List<BooksDTO>>> GetByCategoryIdAsync(Guid categoryId)
        {
            var books = await _bookRepo.GetByCategoryIdAsync(categoryId);
            if (books == null || !books.Any())
                return ApiResponse<List<BooksDTO>>.FailureResponse(_localizer["NotFoundAnyBooks"]);

            var data = _mapper.Map<List<BooksDTO>>(books);
            return ApiResponse<List<BooksDTO>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<List<BooksDTO>>> SearchAsync(BookFilterDto dto)
        {
            var books = _bookRepo.ApplyFilter(dto);
            if (books == null || !books.Any())
                return ApiResponse<List<BooksDTO>>.FailureResponse(_localizer["NotFoundAnyBooks"]);

            var data = _mapper.Map<List<BooksDTO>>(books);
            return ApiResponse<List<BooksDTO>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<List<PopularBookDto>>> PopularAsync()
        {
            var books = await _borrowingRepo.GetBorrowedBooksAsync();
            if (books == null || !books.Any())
                return ApiResponse<List<PopularBookDto>>.FailureResponse(_localizer["NotFoundAnyBooks"]);

            var data = _mapper.Map<List<PopularBookDto>>(books);
            return ApiResponse<List<PopularBookDto>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<bool>> CreateAsync(CreateBookDto model)
        {
            var category = await _categoryRepo.GetByIdAsync(model.CategoryId);
            if (category == null)
                return ApiResponse<bool>.FailureResponse(_localizer["CategoryNotFound"]);

            var author = await _authorRepo.GetByIdAsync(model.AuthorId);
            if (author == null)
                return ApiResponse<bool>.FailureResponse(_localizer["AuthorNotFound"]);

            author.TotalBooks++;
            await _authorRepo.UpdateAsync(author);
            var book = _mapper.Map<Book>(model);
            book.AvailableCopies = model.TotalCopies;
            await _bookRepo.AddAsync(book);

            return ApiResponse<bool>.SuccessResponse(true, _localizer["BookAdded"]);
        }
        public async Task<ApiResponse<bool>> UpdateAsync(UpdateBookDto model)
        {
            var validate = ValidateBookData(model.PublishedYear, model.TotalCopies);
            if (validate != null)
                return ApiResponse<bool>.FailureResponse(validate);

            if (model.AuthorId.HasValue)
            {
                var author = await _authorRepo.GetByIdAsync(model.AuthorId.Value);
                if (author == null)
                    return ApiResponse<bool>.FailureResponse(_localizer["AuthorNotFound"]);
            }
            if (model.CategoryId.HasValue)
            {
                var category = await _categoryRepo.GetByIdAsync(model.CategoryId.Value);
                if (category == null)
                    return ApiResponse<bool>.FailureResponse(_localizer["CategoryNotFound"]);
            }
            var existBook = await _bookRepo.GetByIdAsync(model.Id);
            if (existBook == null)
                return ApiResponse<bool>.FailureResponse(_localizer["BookNotFound"]);

            _mapper.Map(model, existBook);
            await _bookRepo.UpdateAsync(existBook);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["BookUpdated"]);
        }
        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var existBook = await _bookRepo.GetByIdAsync(id);
            if (existBook == null)
                return ApiResponse<bool>.FailureResponse(_localizer["BookNotFound"]);

            await _bookRepo.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["BookDeleted"]);
        }

        #region Private method
        private string? ValidateBookData(int? publishedYear, int? totalCopies)
        {
            if (publishedYear.HasValue && publishedYear > DateTime.UtcNow.Year)
                return _localizer["InvalidPublishedYear"];
            if (totalCopies.HasValue && totalCopies < 0)
                return _localizer["InvalidTotalCopies"];

            return null;
        }

        #endregion
    }
}

using Application.DTOs;
using Application.DTOs.Borrowing;
using Application.DTOs.Fine;
using Application.Enums;
using Application.Event;
using Application.IRepository;
using Application.Response;
using Application.Services.Books;
using Application.Services.RabbitMQ;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services.BorrowBooks
{
    public class BorrowingServices(IBorrowingRepository _borrowingRepo, IUserRepository _userRepository, IBookRepository _bookRepo, IFineRepository _fineRepo, IMapper _mapper, IStringLocalizer<BorrowingServices> _localizer, IRabbitMQPublisher _publish) : IBorrowingService
    {
        public async Task<ApiResponse<List<BorrowingDto>>> GetAllAsync()
        {
            var borrowing = await _borrowingRepo.GetAllAsync();
            if (borrowing == null || !borrowing.Any())
                return ApiResponse<List<BorrowingDto>>.FailureResponse(_localizer["NotFoundAnyBorrowing"]);

            var data = _mapper.Map<List<BorrowingDto>>(borrowing);
            return ApiResponse<List<BorrowingDto>>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<PagedResult<BorrowingDto>>> GetPagedAsync(BorrowingFilterDto dto)
        {
            var borrowing = await _borrowingRepo.GetPagedAsync(dto);
            if (borrowing == null || borrowing.Items == null || !borrowing.Items.Any())
                return ApiResponse<PagedResult<BorrowingDto>>.FailureResponse(_localizer["NotFoundAnyBorrowing"]);

            var items = _mapper.Map<List<BorrowingDto>>(borrowing.Items);
            var paged = new PagedResult<BorrowingDto>
            {
                Items = items,
                PageNumber = borrowing.PageNumber,
                PageSize = borrowing.PageSize,
                TotalCount = borrowing.TotalCount
            };
            return ApiResponse<PagedResult<BorrowingDto>>.SuccessResponse(paged, null);
        }
        public async Task<ApiResponse<List<UserBorrowingDto>>> GetUserBorrowingsAsync(string userId)
        {
            var borrowing = await _borrowingRepo.GetByUserIdAsync(userId);
            if (borrowing == null || !borrowing.Any())
                return ApiResponse<List<UserBorrowingDto>>.FailureResponse(_localizer["NotFoundAnyBorrowing"]);

            return ApiResponse<List<UserBorrowingDto>>.SuccessResponse(_mapper.Map<List<UserBorrowingDto>>(borrowing), null);
        }
        public async Task<ApiResponse<bool>> BorrowBookAsync(BorrowBookDto dto, string userId)
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId);
            if (book == null) return ApiResponse<bool>.FailureResponse(_localizer["BookNotFound"]);

            if (!book.IsAvailableForBorrowing || book.AvailableCopies <= 0)
                return ApiResponse<bool>.FailureResponse(_localizer["BookNotAvailable"]);

            var hasBorrowing = await _borrowingRepo.HasActiveBorrowingAsync(userId, dto.BookId);
            if (hasBorrowing) return ApiResponse<bool>.FailureResponse(_localizer["BookAlreadyBorrowed"]);

            var userExists = await _userRepository.GetByIdAsync(userId);
            if (userExists == null) return ApiResponse<bool>.FailureResponse(_localizer["UserNotFound"]);

            var borrowing = _mapper.Map<Borrowing>(dto);
            borrowing.UserId = userId;
            await _borrowingRepo.AddAsync(borrowing);

            _publish.Publish(new BorrowRequestedEvent
            {
                BorrowingId = borrowing.Id,
                BookTitle = book.Title,
                UserName = borrowing.User.Name,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            }, routingKey: "library.book.borrowed");

            return ApiResponse<bool>.SuccessResponse(true, _localizer["BorrowRequestSent"]);
        }
        public async Task<ApiResponse<bool>> ApproveBorrowingAsync(Guid borrowingId)
        {
            var borrowing = await _borrowingRepo.GetByIdAsync(borrowingId);
            if (borrowing == null) return ApiResponse<bool>.FailureResponse(_localizer["BorrowingNotFound"]);
            if (borrowing.Status != BorrowingStatus.Pending) return ApiResponse<bool>.FailureResponse(_localizer["BookNotBorrowed"]);

            borrowing.Status = BorrowingStatus.Approved;
            borrowing.BorrowDate = DateTime.UtcNow;
            borrowing.DueDate = DateTime.UtcNow.AddMinutes(1);
            borrowing.Book.AvailableCopies--;
            await _borrowingRepo.UpdateAsync(borrowing);

            _publish.Publish(new BorrowingApprovedEvent
            {
                UserId = borrowing.UserId,
                BookTitle = borrowing.Book.Title,
                DueDate = borrowing.DueDate,
                Message = _localizer["BorrowApproved"]
            }, routingKey: "library.book.borrow.approved");
            return ApiResponse<bool>.SuccessResponse(true, _localizer["Success"]);
        }
        public async Task<ApiResponse<bool>> ReturnBookAsync(Guid borrowingId)
        {
            var borrowing = await _borrowingRepo.GetByIdAsync(borrowingId);
            if (borrowing == null) return ApiResponse<bool>.FailureResponse(_localizer["BorrowingNotFound"]);
            if (borrowing.Status != BorrowingStatus.Approved) return ApiResponse<bool>.FailureResponse(_localizer["BookNotBorrowed"]);

            borrowing.Status = BorrowingStatus.Returned;
            borrowing.ReturnDate = DateTime.UtcNow;
            if (borrowing.ReturnDate > borrowing.DueDate)
            {
                var daysLate = (int)Math.Ceiling((borrowing.ReturnDate - borrowing.DueDate).TotalMinutes);
                var fineDto = new CreateFineDto
                {
                    UserId = borrowing.UserId,
                    BorrowingId = borrowingId,
                    DaysOverdue = daysLate,
                    DailyFineRate = 0.5m,
                    FineAmount = daysLate * 0.5m
                };
                var fine = _mapper.Map<Domain.Entities.Fine>(fineDto);
                borrowing.Fine = fine;
                await _fineRepo.AddAsync(fine);
            }

            borrowing.Book.AvailableCopies++;
            await _borrowingRepo.UpdateAsync(borrowing);

            _publish.Publish(new BookReturnedEvent
            {
                UserId = borrowing.UserId,
                BookTitle = borrowing.Book.Title,
                UserName = borrowing.User.Name,
                ReturnDate = borrowing.ReturnDate,
                LateFees = borrowing.Fine?.FineAmount ?? 0,
                Message = _localizer["BookReturned"]
            }, routingKey: "library.book.borrow.returned");

            return ApiResponse<bool>.SuccessResponse(true, _localizer["BookReturned"]);
        }
       
    }
}

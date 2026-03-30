using Application.DTOs;
using Application.Event;
using Application.Helpers;
using Application.IRepository;
using Application.Response;
using Application.Services.RabbitMQ;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Services.Profile
{
    public class UserProfileService(UserManager<User> _userManager , IMapper _mapper, 
        IRabbitMQPublisher _publish, IStringLocalizer<UserProfileService> _localizer) : IUserProfileService
    {
        public async Task<ApiResponse<UserProfileDTO>> GetCurrentUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<UserProfileDTO>.FailureResponse(_localizer["UserNotFound"]);

            var data = _mapper.Map<UserProfileDTO>(user);
            return ApiResponse<UserProfileDTO>.SuccessResponse(data, null);
        }
        public async Task<ApiResponse<bool>> UpdateProfileAsync(UpdateProfileDto model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.FailureResponse(_localizer["UserNotFound"]);

            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);
            _publish.Publish(new UserUpdatedEvent
            {
                UserId = userId,
                Name = model.UserName
            }, routingKey: "account.user.updated");

            return ApiResponse<bool>.SuccessResponse(true, _localizer["ProfileUpdated"]);
        }
        public async Task<ApiResponse<bool>> DeleteProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.FailureResponse(_localizer["UserNotFound"]);

            await _userManager.DeleteAsync(user);
            _publish.Publish(new UserDeletedEvent
            {
                UserId = userId
            }, routingKey: "account.user.deleted");

            return ApiResponse<bool>.SuccessResponse(true, _localizer["ProfileDeleted"]);
        }
    }
}

using Application.Event;
using Application.Helpers;
using Application.IRepository;
using Application.Response;
using Application.Services.Auth.DTOs;
using Application.Services.RabbitMQ;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Register
{
    public class RegisterService(UserManager<User> _userManager, IRabbitMQPublisher _rabbitMQPublisher, IStringLocalizer<RegisterService> _localizer, IMapper _mapper) : IRegisterService
    {
        public async Task<ApiResponse<bool>> RegisterAsync(RegisterModel model)
        {
            var user = await CreateUserAsync(model, "User");
            var message = new UserCreatedEvent
            {
                UserId = user.Data.Id,
                UserName = model.UserName,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };
            _rabbitMQPublisher.Publish(message, routingKey: "account.user.registered");
            return ApiResponse<bool>.SuccessResponse(true, _localizer["RegistrationSuccess"]);
        }
        public async Task<ApiResponse<bool>> RegisterAdminAsync(RegisterModel model)
        {
            var user = await CreateUserAsync(model, "Admin");
            return ApiResponse<bool>.SuccessResponse(true, _localizer["RegistrationSuccess"]);
        }

        #region Private method 
        private async Task<ApiResponse<User>> CreateUserAsync(RegisterModel model, string role)
        {
            var existingEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmail != null)
                return ApiResponse<User>.FailureResponse(_localizer["EmailAlreadyExists"]);

            var existingUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserName != null)
                return ApiResponse<User>.FailureResponse(_localizer["UsernameAlreadyExists"]);

            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return ApiResponse<User>.FailureResponse(_localizer["Error"]);
            await _userManager.AddToRoleAsync(user, role);
            return ApiResponse<User>.SuccessResponse(user, null);
        }
        #endregion
    }
}

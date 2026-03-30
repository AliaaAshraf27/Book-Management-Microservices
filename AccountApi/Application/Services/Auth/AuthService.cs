using Application.DTOs;
using Application.Helpers;
using Application.IRepository;
using Application.Response;
using Application.Services.Auth;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IUserTokenRepository _userTokenRepo;
        private readonly IStringLocalizer _localizer;
        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
                           IOptions<JWT> jwt, IConfiguration configuration, IUserTokenRepository userTokenRepository, IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _userTokenRepo = userTokenRepository;
            _localizer = factory.Create(typeof(AuthService));
        }

        public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return ApiResponse<AuthResponseDTO>.FailureResponse(_localizer["InvalidEmailOrPassword"]);

            var existingToken = await _userTokenRepo.GetTokenAsync(user.Id);

            if (!string.IsNullOrEmpty(existingToken))
            {
                var jwt = new JwtSecurityTokenHandler().ReadToken(existingToken) as JwtSecurityToken;

                if (jwt != null && jwt.ValidTo > DateTime.UtcNow)
                {
                    return ApiResponse<AuthResponseDTO>.SuccessResponse(new AuthResponseDTO
                    {
                        Token = existingToken,
                        ExpireOn = jwt.ValidTo,
                        RefreshToken = await _userTokenRepo.GetRefreshTokenAsync(user.Id)
                    }, null);
                }
            }
            var newTokens = await GenerateNewTokens(user);
            return ApiResponse<AuthResponseDTO>.SuccessResponse(newTokens, null);
        }
        public async Task<ApiResponse<AuthResponseDTO>> RefreshTokenAsync(string refreshToken)
        {
            var userId = await _userTokenRepo.GetUserIdByRefreshTokenAsync(refreshToken);
            if (userId == null)
                return ApiResponse<AuthResponseDTO>.FailureResponse(_localizer["InvalidRefreshToken"]);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<AuthResponseDTO>.FailureResponse(_localizer["UserNotFound"]);

            var tokens = await GenerateNewTokens(user);
            return ApiResponse<AuthResponseDTO>.SuccessResponse(tokens, _localizer["TokenRefreshedSuccessfully"]);
        }
        public async Task<ApiResponse<bool>> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.FailureResponse(_localizer["UserNotFound"]);

            await _userTokenRepo.RemoveTokenAsync(userId);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["LoggedOut"]);
        }
        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.FailureResponse(_localizer["UserNotFound"]);

            if (dto.NewPassword != dto.ConfirmNewPassword)
                return ApiResponse<bool>.FailureResponse(_localizer["PasswordsDoNotMatch"]);

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            return ApiResponse<bool>.SuccessResponse(true, _localizer["ChangePassword"]);
        }
        public async Task<ApiResponse<string>> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return ApiResponse<string>.FailureResponse(_localizer["InvalidRequest"]);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return ApiResponse<string>.SuccessResponse(token, _localizer["PasswordResetToken"]);
        }
        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                return ApiResponse<bool>.FailureResponse(_localizer["PasswordsDoNotMatch"]);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ApiResponse<bool>.FailureResponse(_localizer["InvalidRequest"]);

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
                return ApiResponse<bool>.FailureResponse(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ApiResponse<bool>.SuccessResponse(true, _localizer["PasswordChanged"]);
        }
        #region Private method
        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        { 
            // يتم استرجاع كل ال claims الي موجودة في asp.net identity + roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // بيمنع تكرار نفس الtoken
                new Claim(JwtRegisteredClaimNames.Email, user.Email)

            }.Union(userClaims)
             .Union(roleClaims);
            // بحول مفتاح jwt to list of bytes لزيادة السيكيورتي 
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            // create token 
            return new JwtSecurityToken(
                 issuer: _jwt.Issuer,
                 audience: _jwt.Audience,
                 claims: claims,
                 expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                 signingCredentials: signingCredentials);

        }
        private async Task<AuthResponseDTO> GenerateNewTokens(User user)
        {
            var jwtToken = await CreateJwtToken(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = GenerateRefreshToken();

            await _userTokenRepo.StoreTokenAsync(user.Id, accessToken);
            await _userTokenRepo.StoreRefreshTokenAsync(user.Id, refreshToken);

            return new AuthResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpireOn = jwtToken.ValidTo
            };
        }
        private static string GenerateRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        #endregion
    }
}

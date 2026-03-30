using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IUserTokenRepository
    {
        Task StoreTokenAsync(string userId, string tokenString);
        Task<string?> GetTokenAsync(string userId);
        Task StoreRefreshTokenAsync(string userId, string refreshToken);
        Task<string?> GetRefreshTokenAsync(string userId);
        Task<string?> GetUserIdByRefreshTokenAsync(string refreshToken);
        Task RemoveTokenAsync(string userId);

    }
}

using Application.DTOs;
using Application.IRepository;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserTokenRepository(ApplicationDbContext _dbContext, IDistributedCache _cache)
        : IUserTokenRepository
    {
        public async Task StoreTokenAsync(string userId, string tokenString)
        {
            string cacheKey = $"user:token:{userId}";
            await _cache.SetStringAsync(cacheKey, tokenString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            var existing = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "JWT" && t.Name == "AccessToken");

            if (existing != null)
                existing.Value = tokenString;
            else
            {
                _dbContext.UserTokens.Add(new IdentityUserToken<string>
                {
                    UserId = userId,
                    LoginProvider = "JWT",
                    Name = "AccessToken",
                    Value = tokenString
                });
            }
            await _dbContext.SaveChangesAsync();


        }
        public async Task<string?> GetTokenAsync(string userId)
        {
            string cacheKey = $"user:token:{userId}";
            var cachedToken = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedToken))
                return cachedToken;

            var token = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "JWT" && t.Name == "AccessToken");
            if (token != null)
            {
                await _cache.SetStringAsync(cacheKey, token.Value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }

            return token?.Value;
        }
        public async Task StoreRefreshTokenAsync(string userId, string refreshToken)
        {
            string cacheKey = $"user:refresh_token:{userId}";
            await _cache.SetStringAsync(cacheKey, refreshToken, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });

            var existing = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "JWT" && t.Name == "RefreshToken");

            if (existing != null)
                existing.Value = refreshToken;
            else
            {
                _dbContext.UserTokens.Add(new IdentityUserToken<string>
                {
                    UserId = userId,
                    LoginProvider = "JWT",
                    Name = "RefreshToken",
                    Value = refreshToken
                });
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task<string?> GetRefreshTokenAsync(string userId)
        {
            string cacheKey = $"user:token:{userId}";
            var cachedToken = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedToken))
                return cachedToken;
            var token = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "JWT" && t.Name == "RefreshToken");
            if (token != null)
            {
                await _cache.SetStringAsync(cacheKey, token.Value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
            }

            return token?.Value;
        }
        public async Task<string?> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.Name == "RefreshToken" && t.Value == refreshToken);

            return token?.UserId;
        }
        public async Task RemoveTokenAsync(string userId)
        {
            //Redis ما بيسمحش تبحثي عن القيم (values) بسهولة، هو معمول عشان تستخدمي key-value access فقط.يعني لازم
            //تعرفي المفتاح (key) اللي خزّنتي بيه]
            await _cache.RemoveAsync($"user:token:{userId}");
            await _cache.RemoveAsync($"user:refresh_token:{userId}");
            var tokens = _dbContext.UserTokens
               .Where(t => t.UserId == userId && t.LoginProvider == "JWT");
            _dbContext.UserTokens.RemoveRange(tokens);
            _dbContext.SaveChanges();
        }
        
    }
}


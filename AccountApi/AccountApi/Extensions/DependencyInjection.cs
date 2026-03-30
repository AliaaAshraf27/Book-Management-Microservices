using Application.IRepository;
using Application.Services.Auth;
using Application.Services.Profile;
using Application.Services.RabbitMQ;
using Application.Services.Register;
using Application.Services;
using Infrastructure.Repository;

namespace AccountApi.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddSingleton<IRabbitMQPublisher, RabbitMqPublisher>();

            return services;
        }
    }
}

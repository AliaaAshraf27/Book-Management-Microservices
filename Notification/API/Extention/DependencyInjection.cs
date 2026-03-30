using API.SignalR;
using Application.Handlers;
using Application.IRepository;
using Application.Services.Notifications;
using Infrastructure.Repository;

namespace API.Extention
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISender, Sender>();
            services.AddScoped<UserRegisteredHandle>();
            services.AddScoped<BookReturnedHandler>();
            services.AddScoped<BorrowingApprovedHandler>();
            services.AddScoped<BorrowRequestedHandler>();

            return services;
        }
    }
}

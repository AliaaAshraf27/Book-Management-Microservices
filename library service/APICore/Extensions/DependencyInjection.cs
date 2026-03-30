using Application.IRepository;
using Application.Services.Authors;
using Application.Services.Books;
using Application.Services.BorrowBooks;
using Application.Services.Categories;
using Application.Services.Fine;
using Application.Services.RabbitMQ;
using Application.Services.Reviews;
using Application.Services.Users;
using Infrastructure.Repository;

namespace APICore.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBorrowingRepository, BorrowingRepository>();
            services.AddScoped<IFineRepository, FineRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBorrowingService, BorrowingServices>();
            services.AddScoped<IFineService, FineService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddHostedService<Consumer>();
            services.AddSingleton<IRabbitMQPublisher, RabbitMqPublisher>();

            return services;
        }
    }
}

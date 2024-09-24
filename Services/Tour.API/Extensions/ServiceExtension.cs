using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Tour.API.Persistence;
using Shared.Configurations;
using Infrastructure.Extensions;
using Room.API.Persistence;
using Tour.API.Repositories.Interfaces;
using Tour.API.Repositories;
using Tour.API.Services.Interfaces;
using Tour.API.Services;
using FluentValidation;
using Shared.DTOs;
using Tour.API.Validators;
namespace Tour.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureIdentityDbContext(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configured.");
            Console.WriteLine(databaseSettings.ConnectionString);
            var builder = new MySqlConnectionStringBuilder(databaseSettings.ConnectionString);
            services.AddDbContext<TourDbContext>(option => option.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString)));

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        { 
            return services
                .AddScoped<IValidator<DestinationRequestDTO>, DestinationValidator>()
                .AddScoped<IValidator<ScheduleRequestDTO>, ScheduleValidator>()
                .AddScoped<IValidator<TourRequestDTO>, TourValidator>()
                .AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<IScheduleRepository, ScheduleRepository>()
                .AddScoped<IScheduleService, ScheduleService>()
                .AddScoped<ITourRepository, TourRepository>()
                .AddScoped<ITourService, TourService>()
                .AddScoped<IDestinationRepository, DestinationRepository>()
                .AddScoped<IDestinationService, DestinationService>()
                .AddScoped<TourDbContextSeeder>(); 
        }
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(option =>
                option.AddPolicy("CorsPolicy", cfg =>
                    cfg.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );
            return services;
        }

    }
}

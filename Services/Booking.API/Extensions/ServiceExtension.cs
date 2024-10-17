using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Shared.Configurations;
using Infrastructure.Extensions;
using Booking.API.Persistence;
using Booking.API.Repositories.Interfaces;
using Booking.API.Repositories;
namespace Booking.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,IConfiguration configuration)
        {
            var grpcSettings = configuration.GetSection(nameof(GrpcSettings));
            services.AddSingleton(grpcSettings);
            return services;
        }
        public static IServiceCollection ConfigureIdentityDbContext(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configured.");
            Console.WriteLine(databaseSettings.ConnectionString);
            var builder = new MySqlConnectionStringBuilder(databaseSettings.ConnectionString);
            services.AddDbContext<BookingDbContext>(option => option.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString)));

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                    .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
					.AddScoped<BookingDbContextSeeder>()
                    .AddScoped<IBookingTourRepository,BookingTourRepository>()
                    .AddScoped<IBookingRoomRepository,BookingRoomRepository>();
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

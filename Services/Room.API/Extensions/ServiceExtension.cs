using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Room.API.Persistence;
using Shared.Configurations;
using Infrastructure.Extensions;
using Room.API.Services.Interfaces;
using Room.API.Services;
using Room.API.Repositories;
using Room.API.Repositories.Interfaces;
namespace Room.API.Extensions
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
            services.AddDbContext<RoomDbContext>(option => option.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString)));

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

			return services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
					.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
					.AddScoped<IHotelRepository, HotelRepository>()
					.AddScoped<IRoomRepository, RoomRepository>()
					.AddScoped<IHotelService, HotelService>()
					.AddScoped<IRoomService, RoomService>()
					.AddScoped<IReviewHotelService, ReviewHotelService>()
					.AddScoped<IReviewRoomService, ReviewRoomService>()
					.AddScoped<RoomDbContextSeeder>();
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

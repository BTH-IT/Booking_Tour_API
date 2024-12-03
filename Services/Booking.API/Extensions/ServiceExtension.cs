using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Shared.Configurations;
using Infrastructure.Extensions;
using Booking.API.Persistence;
<<<<<<< HEAD
using Booking.API.Repositories.Interfaces;
using Booking.API.Repositories;
using Booking.API.Services.Interfaces;
using Booking.API.Services;
=======
using EventBus.Masstransit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Booking.API.GrpcClient.Protos;
using Infrastructure.Polly.GprcPolly;
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
<<<<<<< HEAD
					.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
					.AddScoped<IBookingRoomRepository, BookingRoomRepository>()
					.AddScoped<IBookingTourRepository, BookingTourRepository>()
					.AddScoped<IDetailBookingRoomRepository, DetailBookingRoomRepository>()
					.AddScoped<IBookingRoomService, BookingRoomService>()
					.AddScoped<IBookingTourService, BookingTourService>()
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
					.AddScoped<BookingDbContextSeeder>();
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
        public static IServiceCollection AddGrpcClients(this IServiceCollection services)
        {
            var grpcOptions = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));
<<<<<<< HEAD
            services.AddGrpcClient<IdentityGrpcService.IdentityGrpcServiceClient>(options =>
            {
                options.Address = new Uri(grpcOptions.IdentityAddress ?? throw new Exception("Configration Not found"));
            }).AddGrpcCircuitBreakerPolicyHandler();

            services.AddGrpcClient<RoomGrpcService.RoomGrpcServiceClient>(options => {
                options.Address = new Uri(grpcOptions.RoomAddress ?? throw new Exception("Configration Not found"));
            });
            services.AddGrpcClient<TourGrpcService.TourGrpcServiceClient>(options => {
                options.Address = new Uri(grpcOptions.TourAddress ?? throw new Exception("Configuration Not Found"));
            });
=======
            services.AddGrpcClient<IdentityGrpcService.IdentityGrpcServiceClient>(
                option =>
                {
                    option.Address = new Uri(grpcOptions.IdentityAddress ?? throw new Exception("Configration Not found"));
                }
            );
            services.AddGrpcClient<TourGrpcService.TourGrpcServiceClient>(
                option =>
                {
                    option.Address = new Uri(grpcOptions.TourAddress ?? throw new Exception("Configration Not found"));
                }
            );
            services.AddGrpcClient<RoomGrpcService.RoomGrpcServiceClient>(
                option =>
                {
                    option.Address = new Uri(grpcOptions.RoomAddress ?? throw new Exception("Configration Not found"));
                }
            );
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
            return services;
        }
    }
}

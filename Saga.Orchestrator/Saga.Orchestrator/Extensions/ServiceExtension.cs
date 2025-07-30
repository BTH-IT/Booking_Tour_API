using Shared.Configurations;
using Infrastructure.Extensions;
using Saga.Orchestrator.API.GrpcClient.Protos;
using Booking.API.GrpcServer.Protos;
using Saga.Orchestrator.BookingRoomOrderManagers;
using Saga.Orchestrator.BookingTourOrderManagers;
using Saga.Orchestrator.Services.Interfaces;
using Saga.Orchestrator.Services;
using Infrastructure.Configurations;
namespace Saga.Orchestrator.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,IConfiguration configuration)
        {
            var grpcSettings = configuration.GetSection(nameof(GrpcSettings));
            services.AddSingleton(grpcSettings);
            services.Configure<SMTPEmailSetting>(configuration.GetSection("SMTPEmailSettings"));
            
            return services;
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
            services.AddGrpcClient<BookingGrpcService.BookingGrpcServiceClient>(options =>
            {
                options.Address = new Uri(grpcOptions.BookingAddress ?? throw new Exception("Configration Not found"));
            });
            return services;
        }
        public static IServiceCollection AddInfrastructures(this IServiceCollection services)
        {
            services.AddTransient<BookingRoomManager>();
            services.AddTransient<BookingTourManager>();
            services.AddTransient<IEmailTemplateService, BookingEmailService>();
            return services;
        }
    }
}

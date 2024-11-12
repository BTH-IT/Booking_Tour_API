using Shared.Configurations;
namespace Realtime.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,IConfiguration configuration)
        {
            var grpcSettings = configuration.GetSection(nameof(GrpcSettings));
            services.AddSingleton(grpcSettings);
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
    }
}

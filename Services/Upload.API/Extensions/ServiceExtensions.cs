
using CloudinaryDotNet;

namespace Upload.API.Extensions
{
    public static class ServiceExtensions
    {
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
        public static IServiceCollection ConfigureCloudinary(this IServiceCollection services)
        {
            var cloudinaryAccount = new Account(
                    Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                );
            Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);
            return services;
        }
    }
}

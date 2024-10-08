using Amazon.S3;
using Amazon.Util;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
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
        public static IServiceCollection AddAwsStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetAWSOptions("AWS");
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
            return services;
        }
    }
}

using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureOcelot(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddOcelot(configuration)
                .AddCacheManager(c=>c.WithDictionaryHandle());
            services.AddSwaggerForOcelot(configuration,
                x=>x.GenerateDocsForGatewayItSelf = false);
            return services;
        }
        public static IServiceCollection ConfigureCors(this IServiceCollection services,IConfiguration configuration)
        {
            var origins = configuration["AllowOrigins"];
            services.AddCors(option =>
                option.AddPolicy("CorsPolicy", cfg =>
                    cfg.WithOrigins(origins ?? "*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );
            return services;
        }

    }
}

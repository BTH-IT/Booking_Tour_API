using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
<<<<<<< HEAD
using Ocelot.Provider.Polly;
=======
using System.Configuration;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureOcelot(this IServiceCollection services,IConfiguration configuration,string environment)
        {
  
            var configurationBuilder = new ConfigurationBuilder()
                    .AddConfiguration(configuration)  
                    .SetBasePath(Directory.GetCurrentDirectory()) 
                    .AddJsonFile($"ocelot.{environment}.json", optional: true, reloadOnChange: true);

            var ocelotConfiguration = configurationBuilder.Build();

            services.AddOcelot(ocelotConfiguration)
<<<<<<< HEAD
                .AddPolly()
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                .AddCacheManager(c=>c.WithDictionaryHandle());

            services.AddSwaggerForOcelot(ocelotConfiguration,
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
                )
            );
            return services;
        }

    }
}

﻿using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

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
                .AddPolly()
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

﻿using Common.Logging;
using Serilog;

namespace OcelotApiGw.Extensions
{
    public static class HostExtension
    {
        public static void AddAppConfigurations(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                        reloadOnChange: true)
                    .AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: false,
                        reloadOnChange: true)
                    .AddEnvironmentVariables();
            builder.Host.UseSerilog(SeriLogger.Configure);
        }
    }
}

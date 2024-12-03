<<<<<<< HEAD
﻿using Common.Logging;
=======
﻿
using Common.Logging;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Serilog;

namespace Booking.API.Extensions
{
    public static class HostExtension
    {
        public static void AddAppConfigurations(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                        reloadOnChange: true)
                    .AddEnvironmentVariables();
            builder.Host.UseSerilog(SeriLogger.Configure);
        }
    }
}

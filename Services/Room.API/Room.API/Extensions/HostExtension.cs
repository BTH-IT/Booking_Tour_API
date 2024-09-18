using Common.Logging;
using Serilog;

namespace Room.API.Extensions
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

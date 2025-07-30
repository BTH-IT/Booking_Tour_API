using Contracts.ScheduleJobs;
using Contracts.Services;
using Hangfire.API.Services;
using Hangfire.API.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Infrastructure.ScheduleJobs;
using Infrastructure.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Shared.Configurations;

namespace Hangfire.API.Extensions
{
    public static class ServiceExtension
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangFireSettings = configuration.GetSection(nameof(HangFireSettings))
                .Get<HangFireSettings>();
            services.AddSingleton(hangFireSettings);

            var emailSettings = configuration.GetSection("SMTPEmailSettings")
                .Get<SMTPEmailSetting>();

            services.AddSingleton(emailSettings);

            return services;
        }
        public static IServiceCollection ConfigureHealthCheck(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<HangFireSettings>(nameof(HangFireSettings));

            services.AddSingleton(sp=>new MongoClient(databaseSettings.Storage.ConnectionString))
                .AddHealthChecks()
                .AddMongoDb(databaseNameFactory: sp=> "hangfire-webapi");

            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
            => services.AddTransient<IScheduleJobService, HangFireService>()
                .AddScoped<ISmtpEmailService, SmtpEmailService>()
                .AddScoped<IBackgroundJobService, BackgroundJobService>()
        ;
    }
}

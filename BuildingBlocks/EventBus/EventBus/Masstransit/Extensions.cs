using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.Extensions;
namespace EventBus.Masstransit
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,
    IWebHostEnvironment env, Assembly assembly)
        {
            services.AddMassTransit(configure => { SetupMasstransitConfigurations(services, configure, assembly); });

            return services;
        }

        private static void SetupMasstransitConfigurations(IServiceCollection services,
            IBusRegistrationConfigurator configure, Assembly assembly)
        {
            configure.AddConsumers(assembly);
            configure.AddSagaStateMachines(assembly);
            configure.AddSagas(assembly);
            configure.AddActivities(assembly);

            configure.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqOptions = services.GetOptions<RabbitMqOptions>(nameof(RabbitMqOptions));
                if (rabbitMqOptions == null) 
                {
                    throw new Exception("Missing RabbitMq options");
                }
                configurator.Host(rabbitMqOptions.HostName,rabbitMqOptions.Port,"/", h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });
                configurator.ConfigureEndpoints(context);

                configurator.UseMessageRetry(AddRetryConfiguration);
            });
        }
        private static void AddRetryConfiguration(IRetryConfigurator retryConfigurator)
        {
            retryConfigurator.Exponential(
                    3,
                    TimeSpan.FromMilliseconds(200),
                    TimeSpan.FromMinutes(120),
                    TimeSpan.FromMilliseconds(200));
        }
    }
}

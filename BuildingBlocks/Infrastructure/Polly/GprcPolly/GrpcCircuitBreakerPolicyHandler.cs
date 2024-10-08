using Ardalis.GuardClauses;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations;
using ILogger = Serilog.ILogger;
using Polly;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Polly.GprcPolly
{
    public static class GrpcCircuitBreakerPolicyHandler
    {
        public static IHttpClientBuilder AddGrpcCircuitBreakerPolicyHandler(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddPolicyHandler((sp, _) =>
            {
                var options = sp.GetRequiredService<IConfiguration>().GetOptionsV2<PolicySettings>(nameof(PolicySettings));

                Guard.Against.Null(options, nameof(options));

                var logger = sp.GetRequiredService<ILogger>();

                return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: options.CircuitBreaker.RetryCount,
                        durationOfBreak: TimeSpan.FromSeconds(options.CircuitBreaker.BreakDuration),
                        onBreak: (response, breakDuration) =>
                        {
                            if (response?.Exception != null)
                            {
                                logger.Error(response.Exception,
                                    "Service shutdown during {BreakDuration} after {RetryCount} failed retries",
                                    breakDuration,
                                    options.CircuitBreaker.RetryCount);
                            }
                        },
                        onReset: () =>
                        {
                            logger.Information("Service restarted");
                        });
            });
        }
    }
}

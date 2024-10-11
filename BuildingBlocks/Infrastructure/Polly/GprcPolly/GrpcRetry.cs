using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using Ardalis.GuardClauses;
using Shared.Configurations;
using Polly;
using ILogger = Serilog.ILogger;
namespace Infrastructure.Polly.GprcPolly
{
    public static class GrpcRetry
    {
        public static IHttpClientBuilder AddGrpcRetryPolicyHandler(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddPolicyHandler((sp, _) =>
            {
                var options = sp.GetRequiredService<IConfiguration>().GetOptionsV2<PolicySettings>(nameof(PolicySettings));

                Guard.Against.Null(options, nameof(options));

                return Policy
                    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(options.Retry.RetryCount,
                        retryAttempt => TimeSpan.FromSeconds(options.Retry.SleepDuration),
                        onRetry: (response, timeSpan, retryCount, context) =>
                        {
                            if (response?.Exception != null)
                            {
                                var logger = sp.GetRequiredService<ILogger>();

                                logger.Error(response.Exception,
                                    "Request failed with {StatusCode}. Waiting {TimeSpan} before next retry. Retry attempt {RetryCount}.",
                                    response.Result.StatusCode,
                                    timeSpan,
                                    retryCount);
                            }
                        });
            });
        }
    }
}

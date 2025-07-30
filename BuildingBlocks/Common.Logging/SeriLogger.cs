using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Configurations;

namespace Common.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
       (context, configuration) =>
       {
           var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
           var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

           var elasticConfigJson = context.Configuration.GetSection("ElasticConfigurations");

           var elasticConfig = new ElasticConfigurations();

           elasticConfigJson.Bind(elasticConfig);

           configuration
               .WriteTo.Debug()
               .WriteTo.Console(outputTemplate:
                   "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
               .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticConfig.Uri))
               {
                   IndexFormat = $"BookingSystem-{applicationName}-{environmentName}-{DateTime.UtcNow:yyyy-MM}",
                   AutoRegisterTemplate = true,
                   NumberOfReplicas = 1,
                   NumberOfShards = 2,
                   ModifyConnectionSettings = x =>x.BasicAuthentication(elasticConfig.Username,elasticConfig.Password)
               })
               .Enrich.FromLogContext()
               .Enrich.WithMachineName()
               .Enrich.WithProperty("Environment", environmentName)
               .Enrich.WithProperty("Application", applicationName)
               .ReadFrom.Configuration(context.Configuration);
       };
    }
}

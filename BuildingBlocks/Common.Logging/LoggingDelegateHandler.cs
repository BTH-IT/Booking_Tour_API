using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ILogger = Serilog.ILogger;
namespace Common.Logging
{
    public class LoggingDelegateHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        public LoggingDelegateHandler(ILogger logger) 
        {
            _logger = logger;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Sending request {Url} - Method {Method} - Version {Version}",
                    request.RequestUri,
                    request.Method,
                    request.Version);

                var response =  base.SendAsync(request, cancellationToken);
                if (response.IsCompletedSuccessfully)
                {
                    _logger.Information("Sending request {Url} - Method {Method} - Version {Version} - Success",
                        request.RequestUri,
                        request.Method,
                        request.Version);
                }
                else
                {
                    _logger.Information("Sending request {Url} - Method {Method} - Version {Version} -Failed",
                        request.RequestUri,
                        request.Method,
                        request.Version);
                }
                return response;

            }
            catch (Exception ex) 
            {
                _logger.Error("Sending request {Url} - Method {Method} - Version {Version} - Error {Error}",
                       request.RequestUri,
                        request.Method,
                        request.Version,
                        ex.Message);
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway)
                {
                    RequestMessage = request
                });

            }

        }
    }
}

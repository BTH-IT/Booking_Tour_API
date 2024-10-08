using Grpc.Core;
using Grpc.Core.Interceptors;
using ILogger = Serilog.ILogger;
namespace Contracts.Exceptions
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        private readonly ILogger _logger;   
        public GrpcExceptionInterceptor(ILogger logger) 
        {
            _logger = logger;   
        }   
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.Error("Error In Grpc");
            try
            {

                return await continuation(request, context);
            }
            catch (System.Exception exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, exception.Message));
            }
        }
    }
}

using Grpc.Core;
using Identity.API.GrpcServer.Protos;
using System.Net.WebSockets;

namespace Identity.API.GrpcServer.Services
{
    public record GetUserById(int Result);
    public class IdentityProtoService : IdentityGrpcService.IdentityGrpcServiceBase
    {
        public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetUserByIdResponse()
            {
                Result = 100
            });
        }
    }
}

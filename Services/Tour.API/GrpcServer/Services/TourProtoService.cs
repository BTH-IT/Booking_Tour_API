using Grpc.Core;
using Tour.API.GrpcServer.Protos;

namespace Tour.API.GrpcServer.Services
{
    public class TourProtoService : TourGrpcService.TourGrpcServiceBase
    {
        public override Task<GetTourByIdResponse> GetTourById(GetTourByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetTourByIdResponse
            {
                Result = 102
            });
        }
    }
}

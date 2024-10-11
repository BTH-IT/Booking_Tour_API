using Grpc.Core;
using Room.API.GrpcServer.Protos;

namespace Room.API.GrpcServer.Services
{
    public class RoomProtoService : RoomGrpcService.RoomGrpcServiceBase
    {
        public override Task<GetRoomByIdResponse> GetRoomById(GetRoomByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetRoomByIdResponse()
            {
                Result = 101
            });
        }
    }
}

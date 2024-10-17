using Grpc.Core;
using Room.API.GrpcServer.Protos;
using Room.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
namespace Room.API.GrpcServer.Services
{
    public class RoomProtoService : RoomGrpcService.RoomGrpcServiceBase
    {
        private readonly IRoomRepository roomRepository;
        private readonly ILogger logger;

        public RoomProtoService(IRoomRepository roomRepository, ILogger logger)
        {
            this.roomRepository = roomRepository;
            this.logger = logger;
        }
        public override async Task<GetRoomsByIdsResponse> GetRoomsByIds(GetRoomsByIdsRequest request, ServerCallContext context)
        {
            logger.Information("Begin : GetRoomsByIds - RoomGrpcServer");
            var response = new GetRoomsByIdsResponse();
            foreach(var item in request.Ids)
            {
                var room = await roomRepository.GetByIdAsync(item);
                if(room != null)
                {
                    var roomResponse = new RoomResponse()
                    {
                        Id = room.Id,
                        Price = room.Price, 
                        IsAvailable = room.IsAvailable, 
                        Name = room.Name,
                    }; 
                    response.Rooms.Add(roomResponse);
                }    
            }
            logger.Information("End : GetRoomsByIds - RoomGrpcServer");
            return response;
       
        }
    }
}

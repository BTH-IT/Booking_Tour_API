using AutoMapper;
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
        private readonly IMapper mapper;
        public RoomProtoService(IRoomRepository roomRepository, 
            ILogger logger,
            IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.logger = logger;
            this.mapper = mapper;
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
                    response.Rooms.Add(mapper.Map<RoomResponse>(room));
                }    
            }
            logger.Information("End : GetRoomsByIds - RoomGrpcServer");
            return response;
       
        }

        public override async Task<UpdateRoomsAvailabilityResponse> UpdateRoomsAvailability(UpdateRoomsAvailabilityRequest request, ServerCallContext context)
        {
            logger.Information("Begin : UpdateRoomsAvailability - RoomGrpcServer");
            var response = new UpdateRoomsAvailabilityResponse();
            response.Result = true;
            foreach (var item in request.Ids)
            {
               var room = await roomRepository.GetRoomByIdAsync(item);
                if(room == null)
                {
                    response.Result = false;
                    break;
                }
                room.IsAvailable = request.IsAvailable;
                var updateResult = await roomRepository.UpdateRoomAsync(room);
                if(updateResult < 0 )
                {
                    response.Result = false;
                    break;
                }    
            }
            logger.Information("End : UpdateRoomsAvailability - RoomGrpcServer");
            return response;
        }
    }
}

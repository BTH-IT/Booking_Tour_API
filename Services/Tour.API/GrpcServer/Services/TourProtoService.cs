using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Net.WebSockets;
using Tour.API.GrpcServer.Protos;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services;

namespace Tour.API.GrpcServer.Services
{
    public class TourProtoService : TourGrpcService.TourGrpcServiceBase
    {
        private readonly IScheduleRepository scheduleRepository;
        public TourProtoService(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }

        public override async Task<GetScheduleByIdResponse> GetScheduleById(GetScheduleByIdRequest request, ServerCallContext context)
        {
            var tour = await scheduleRepository.GetScheduleByIdAsync(request.Id);
            if (tour == null)
                return null;
            var response = new GetScheduleByIdResponse();
            response.Id = request.Id;
            response.AvailableSeats =  tour.AvailableSeats;
            if(tour.DateStart.HasValue)
            {
                response.DateStart = Timestamp.FromDateTime(tour.DateStart.Value.ToUniversalTime());
            }

            if (tour.DateEnd.HasValue)
            {
                response.DateEnd = Timestamp.FromDateTime(tour.DateEnd.Value.ToUniversalTime());
            }

            return response;    
        }
    }
}

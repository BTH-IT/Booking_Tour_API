using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Net.WebSockets;
using Tour.API.GrpcServer.Protos;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services;
using ILogger = Serilog.ILogger;
namespace Tour.API.GrpcServer.Services
{
    public class TourProtoService : TourGrpcService.TourGrpcServiceBase
    {
        private readonly IScheduleRepository scheduleRepository;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        public TourProtoService(IScheduleRepository scheduleRepository,
            ILogger logger,
            IMapper mapper)
        {
            this.scheduleRepository = scheduleRepository;
            this.logger = logger;
            this.mapper = mapper;   
        }

        public override async Task<GetScheduleByIdResponse> GetScheduleById(GetScheduleByIdRequest request, ServerCallContext context)
        {
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.Id);
            var response  = new GetScheduleByIdResponse()
            {
                Schedule = mapper.Map<ScheduleResponse>(schedule)
            };  
            return response;
        }

        public override async Task<UpdateScheduleAvailableSeatResponse> UpdateScheduleAvailableSeat(UpdateScheduleAvailableSeatRequest request, ServerCallContext context)
        {
            var response = new UpdateScheduleAvailableSeatResponse();
            
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.ScheduleId);
            if (schedule == null) 
            {
                response.Result = false;
                response.Message = "Không tìm thấy thông tin lịch trình";
                return response;
            }
            if (schedule.AvailableSeats - request.Count < 0)
            {
                response.Result = false;
                response.Message = "Lịch trình còn thiếu chỗ";
                return response;
            }

            schedule.AvailableSeats = schedule.AvailableSeats -  request.Count;
            
            var result = await scheduleRepository.UpdateAsync(schedule);
            if(result > 0)
            {
                response.Result = true;
                response.Message = "Cập nhật thành công";
            }
            else
            {
                response.Result = false;
                response.Message = "Cập nhật thất bại";
            }
            return response;
        }
    }
}

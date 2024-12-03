<<<<<<< HEAD
﻿using AutoMapper;
using EventBus.IntergrationEvents.Events;
using Grpc.Core;
using MassTransit;
using Tour.API.GrpcServer.Protos;
using Tour.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
=======
﻿using Grpc.Core;
using Tour.API.GrpcServer.Protos;

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
namespace Tour.API.GrpcServer.Services
{
    public class TourProtoService : TourGrpcService.TourGrpcServiceBase
    {
<<<<<<< HEAD
        private readonly IScheduleRepository scheduleRepository;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public TourProtoService(IScheduleRepository scheduleRepository,
            ILogger logger,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            this.scheduleRepository = scheduleRepository;
            this.logger = logger;
            this.mapper = mapper;   
            _publishEndpoint = publishEndpoint;
        }

        public override async Task<GetScheduleByIdResponse> GetScheduleById(GetScheduleByIdRequest request, ServerCallContext context)
        {
            logger.Information("START -- TourProtoService -- GetScheduleById");
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.Id);
            var response  = new GetScheduleByIdResponse()
            {
                Schedule = mapper.Map<ScheduleResponse>(schedule)
            };
            logger.Information("END -- TourProtoService -- GetScheduleById");

            return response;
        }

        public override async Task<GetSchedulesByIdsResponse> GetSchedulesByIds(GetSchedulesByIdsRequest request, ServerCallContext context)
        {
            logger.Information("START -- TourProtoService -- GetSchedulesByIds");

            var scheduleIds = request.Ids.Distinct().ToList();
            var response = new GetSchedulesByIdsResponse();
            foreach ( var scheduleId in scheduleIds)
            {
                var schedule = await scheduleRepository.GetScheduleByIdAsync(scheduleId);
                if(schedule != null)
                {
                    response.Schedules.Add(mapper.Map<ScheduleResponse>(schedule));
                }    

            }
            logger.Information("END -- TourProtoSxervice -- GetSchedulesByIds");
            return response;    

        }

        public override async Task<UpdateScheduleAvailableSeatResponse> UpdateScheduleAvailableSeat(UpdateScheduleAvailableSeatRequest request, ServerCallContext context)
        {
            logger.Information("START -- TourProtoService -- UpdateScheduleAvailableSeat");

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
                await _publishEndpoint.Publish(new ScheduleUpdateEvent()
                {
                    Id = Guid.NewGuid(),
                    ObjectId = schedule.Id,
                    Data = schedule.AvailableSeats,
                    CreationDate = DateTime.Now,
                });
            }
            else
            {
                response.Result = false;
                response.Message = "Cập nhật thất bại";
            }
            logger.Information("END -- TourProtoService -- UpdateScheduleAvailableSeat");

            return response;
=======
        public override Task<GetTourByIdResponse> GetTourById(GetTourByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetTourByIdResponse
            {
                Result = 102
            });
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        }
    }
}

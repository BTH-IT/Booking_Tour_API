using AutoMapper;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Entities;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace Tour.API.Services
{
	public class ScheduleService : IScheduleService
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IPublishEndpoint _publishEndpoint;
		public ScheduleService(IScheduleRepository scheduleRepository, 
			IMapper mapper, 
			ILogger logger,
			IPublishEndpoint publishEndpoint)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
			_logger = logger;
			_publishEndpoint = publishEndpoint;
		}

		public async Task<ApiResponse<List<ScheduleResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: ScheduleService - GetAllAsync");
			try
			{
				var schedules = await _scheduleRepository.GetSchedulesAsync();
				var data = _mapper.Map<List<ScheduleResponseDTO>>(schedules);
				_logger.Information("End: ScheduleService - GetAllAsync");
				return new ApiResponse<List<ScheduleResponseDTO>>(200, data, "Successfully retrieved the schedule list.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<ScheduleResponseDTO>>(500, null, $"An error occurred while retrieving the schedule list: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ScheduleResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: ScheduleService - GetByIdAsync, id: {id}");
			try
			{
				var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
				if (schedule == null)
				{
					_logger.Information($"Schedule not found, id: {id}");
					return new ApiResponse<ScheduleResponseDTO>(404, null, $"Schedule not found, id: {id}");
				}
				var data = _mapper.Map<ScheduleResponseDTO>(schedule);
				_logger.Information("End: ScheduleService - GetByIdAsync");
				return new ApiResponse<ScheduleResponseDTO>(200, data, "Successfully retrieved the schedule data.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<ScheduleResponseDTO>(500, null, $"An error occurred while retrieving the schedule: {ex.Message}");
			}
		}

		public async Task<ApiResponse<List<ScheduleResponseDTO>>> GetByTourIdAsync(int tourId)
		{
			_logger.Information($"Begin: ScheduleService - GetByTourIdAsync, tourId: {tourId}");
			try
			{
				var schedules = await _scheduleRepository.GetSchedulesByTourIdAsync(tourId);
				if (schedules == null)
				{
					_logger.Information($"Schedules not found for tourId: {tourId}");
					return new ApiResponse<List<ScheduleResponseDTO>>(404, null, $"Schedules not found for tourId: {tourId}");
				}
				var data = _mapper.Map<List<ScheduleResponseDTO>>(schedules);
				_logger.Information("End: ScheduleService - GetByTourIdAsync");
				return new ApiResponse<List<ScheduleResponseDTO>>(200, data, "Successfully retrieved the schedule data.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - GetByTourIdAsync: {ex.Message}", ex);
				return new ApiResponse<List<ScheduleResponseDTO>>(500, null, $"An error occurred while retrieving the schedule by tour: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ScheduleResponseDTO>> CreateAsync(ScheduleRequestDTO item)
		{
			_logger.Information("Begin: ScheduleService - CreateAsync");
			try
			{
				var scheduleEntity = _mapper.Map<Schedule>(item);

				var result = await _scheduleRepository.CreateScheduleAsync(scheduleEntity);

				if (result <= 0)
				{
					_logger.Warning("Failed to create schedule");
					return new ApiResponse<ScheduleResponseDTO>(400, null, "Error occurred while creating the schedule.");
				}

				var createdSchedule = await _scheduleRepository.GetScheduleByIdAsync(result);
				var responseData = _mapper.Map<ScheduleResponseDTO>(createdSchedule);

				_logger.Information("End: ScheduleService - CreateAsync");
				return result > 0
					? new ApiResponse<ScheduleResponseDTO>(200, responseData, "Successfully created the schedule.")
					: new ApiResponse<ScheduleResponseDTO>(400, null, "Failed to create the schedule.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<ScheduleResponseDTO>(500, null, $"An error occurred while creating the schedule: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ScheduleResponseDTO>> UpdateAsync(int id, ScheduleRequestDTO item)
		{
			_logger.Information($"Begin: ScheduleService - UpdateAsync, id: {id}");
			try
			{
				var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);

				if (schedule == null)
				{
					_logger.Information($"Schedule not found, id: {id}");
					return new ApiResponse<ScheduleResponseDTO>(404, null, $"Schedule not found, id: {id}");
				}

				_mapper.Map(item, schedule);
				schedule.UpdatedAt = DateTime.UtcNow;

				var result = await _scheduleRepository.UpdateScheduleAsync(schedule);

				if (result <= 0)
				{
					_logger.Warning("Failed to update schedule");
					return new ApiResponse<ScheduleResponseDTO>(400, null, "Error occurred while updating the schedule.");
				}

				var updatedSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
				
                var responseData = _mapper.Map<ScheduleResponseDTO>(updatedSchedule);
				_logger.Information("End: ScheduleService - UpdateAsync");
				// publish event
				await _publishEndpoint.Publish(new ScheduleUpdateEvent()
				{
					Id = Guid.NewGuid(),	
					ObjectId  = schedule.Id,
					Data = schedule.AvailableSeats,
					CreationDate = DateTime.Now,
				});
				return result > 0
					? new ApiResponse<ScheduleResponseDTO>(200, responseData, "Successfully updated the schedule.")
					: new ApiResponse<ScheduleResponseDTO>(400, null, "Failed to update the schedule.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<ScheduleResponseDTO>(500, null, $"An error occurred while updating the schedule: {ex.Message}");
			}
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: ScheduleService - DeleteAsync, id: {id}");
			try
			{
				var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
				if (schedule == null)
				{
					_logger.Information($"Schedule not found, id: {id}");
					return new ApiResponse<int>(404, 0, $"Schedule not found, id: {id}");
				}

				_scheduleRepository.Delete(schedule);
				var result = await _scheduleRepository.SaveChangesAsync();
				_logger.Information("End: ScheduleService - DeleteAsync");

				return result > 0
					? new ApiResponse<int>(200, result, "Successfully deleted the schedule.")
					: new ApiResponse<int>(400, result, "Failed to delete the schedule.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in ScheduleService - DeleteAsync: {ex.Message}", ex);
				return new ApiResponse<int>(500, 0, $"An error occurred while deleting the schedule: {ex.Message}");
			}
		}
	}
}

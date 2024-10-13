using AutoMapper;
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

        public ScheduleService(IScheduleRepository scheduleRepository, IMapper mapper, ILogger logger)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<ScheduleResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: ScheduleService - GetAllAsync");
            var schedules = await _scheduleRepository.GetSchedulesAsync();
            var data = _mapper.Map<List<ScheduleResponseDTO>>(schedules);
            _logger.Information("End: ScheduleService - GetAllAsync");
            return new ApiResponse<List<ScheduleResponseDTO>>(200, data, "Lấy danh sách lịch trình thành công");
        }

        public async Task<ApiResponse<ScheduleResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin: ScheduleService - GetByIdAsync, id: {id}");
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                _logger.Information($"Schedule not found, id: {id}");
                return new ApiResponse<ScheduleResponseDTO>(404, null, "Không tìm thấy lịch trình");
            }
            var data = _mapper.Map<ScheduleResponseDTO>(schedule);
            _logger.Information("End: ScheduleService - GetByIdAsync");
            return new ApiResponse<ScheduleResponseDTO>(200, data, "Lấy dữ liệu lịch trình thành công");
        }

		public async Task<ApiResponse<List<ScheduleResponseDTO>>> GetByTourIdAsync(int tourId)
		{
			_logger.Information($"Begin: ScheduleService - GetByIdAsync, id: {tourId}");
			var schedules = await _scheduleRepository.GetSchedulesByTourIdAsync(tourId);
			if (schedules == null)
			{
				_logger.Information($"Schedule not found, id: {tourId}");
				return new ApiResponse<List<ScheduleResponseDTO>>(404, null, "Không tìm thấy lịch trình");
			}
			var data = _mapper.Map<List<ScheduleResponseDTO>>(schedules);
			_logger.Information("End: ScheduleService - GetByIdAsync");
			return new ApiResponse<List<ScheduleResponseDTO>>(200, data, "Lấy dữ liệu lịch trình thành công");
		}

		public async Task<ApiResponse<ScheduleResponseDTO>> CreateAsync(ScheduleRequestDTO item)
        {
            _logger.Information("Begin: ScheduleService - CreateAsync");

            var scheduleEntity = _mapper.Map<Schedule>(item);
            
            var result = await _scheduleRepository.CreateAsync(scheduleEntity);

			var createdSchedule = await _scheduleRepository.GetScheduleByIdAsync(result);
			var responseData = _mapper.Map<ScheduleResponseDTO>(createdSchedule);

			_logger.Information("End: ScheduleService - CreateAsync");
            return result > 0
                ? new ApiResponse<ScheduleResponseDTO>(200, responseData, "Tạo lịch trình thành công")
                : new ApiResponse<ScheduleResponseDTO>(400, null, "Tạo lịch trình thất bại");
        }

        public async Task<ApiResponse<ScheduleResponseDTO>> UpdateAsync(int id, ScheduleRequestDTO item)
        {
            _logger.Information($"Begin: ScheduleService - UpdateAsync, id: {id}");

            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);

            if (schedule == null)
            {
                _logger.Information($"Schedule not found, id: {id}");
                return new ApiResponse<ScheduleResponseDTO>(404, null, "Không tìm thấy lịch trình");
            }

			schedule = _mapper.Map<Schedule>(item);
			schedule.Id = id;
			schedule.UpdatedAt = DateTime.UtcNow;


			var result = await _scheduleRepository.UpdateAsync(schedule);
			var updatedSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
			var responseData = _mapper.Map<ScheduleResponseDTO>(updatedSchedule);
			_logger.Information("End: ScheduleService - UpdateAsync");

            return result > 0
                ? new ApiResponse<ScheduleResponseDTO>(200, responseData, "Cập nhật lịch trình thành công")
                : new ApiResponse<ScheduleResponseDTO>(400, null, "Cập nhật lịch trình thất bại");
        }

        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin: ScheduleService - DeleteAsync, id: {id}");
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                _logger.Information($"Schedule not found, id: {id}");
                return new ApiResponse<int>(404, 0, "Không tìm thấy lịch trình cần xóa");
            }

            _scheduleRepository.Delete(schedule);
            var result = await _scheduleRepository.SaveChangesAsync();
            _logger.Information("End: ScheduleService - DeleteAsync");

            return result > 0
                ? new ApiResponse<int>(200, result, "Xóa lịch trình thành công")
                : new ApiResponse<int>(400, result, "Xóa lịch trình thất bại");
        }
    }
}

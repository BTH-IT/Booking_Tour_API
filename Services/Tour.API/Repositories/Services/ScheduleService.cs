using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var schedules = await _scheduleRepository.FindAll().ToListAsync();
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

		public async Task<ApiResponse<int>> CreateAsync(ScheduleRequestDTO item)
        {
            _logger.Information("Begin: ScheduleService - CreateAsync");

            var scheduleEntity = _mapper.Map<Schedule>(item);
            
            var result = await _scheduleRepository.CreateAsync(scheduleEntity);

            _logger.Information("End: ScheduleService - CreateAsync");

            return result > 0
                ? new ApiResponse<int>(200, scheduleEntity.Id, "Tạo lịch trình thành công")
                : new ApiResponse<int>(400, 0, "Tạo lịch trình thất bại");
        }

        public async Task<ApiResponse<ScheduleResponseDTO>> UpdateAsync(ScheduleRequestDTO item)
        {
            _logger.Information($"Begin: ScheduleService - UpdateAsync, id: {item.Id}");

            var schedule = await _scheduleRepository.GetScheduleByIdAsync(item.Id.Value);

            if (schedule == null)
            {
                _logger.Information($"Schedule not found, id: {item.Id}");
                return new ApiResponse<ScheduleResponseDTO>(404, null, "Không tìm thấy lịch trình");
            }

            _mapper.Map(item, schedule);
            var result = await _scheduleRepository.SaveChangesAsync();

            _logger.Information("End: ScheduleService - UpdateAsync");

            return result > 0
                ? new ApiResponse<ScheduleResponseDTO>(200, _mapper.Map<ScheduleResponseDTO>(schedule), "Cập nhật lịch trình thành công")
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

﻿using AutoMapper;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Entities;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace Tour.API.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IScheduleService _scheduleService;

		public TourService(ITourRepository tourRepository, IScheduleService scheduleService, IMapper mapper, ILogger logger)
        {
            _tourRepository = tourRepository;
			_scheduleService = scheduleService;
			_mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TourResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: TourService - GetAllAsync");
            var tours = await _tourRepository.GetToursAsync();
            var data = _mapper.Map<List<TourResponseDTO>>(tours);
            _logger.Information("End: TourService - GetAllAsync");
            return new ApiResponse<List<TourResponseDTO>>(200, data, "Lấy danh sách tour thành công");
                                                
        }

        public async Task<ApiResponse<TourResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin: TourService - GetByIdAsync, id: {id}");
            var tour = await _tourRepository.GetTourByIdAsync(id);
            if (tour == null)
            {
                _logger.Information($"Tour not found, id: {id}");
                return new ApiResponse<TourResponseDTO>(404, null, "Không tìm thấy tour");
            }
            var data = _mapper.Map<TourResponseDTO>(tour);
            _logger.Information("End: TourService - GetByIdAsync");
            return new ApiResponse<TourResponseDTO>(200, data, "Lấy dữ liệu tour thành công");
        }

		public async Task<ApiResponse<TourResponseDTO>> CreateAsync(TourRequestDTO item)
		{
			_logger.Information("Begin: TourService - CreateAsync");

			try
			{
				var tourEntity = _mapper.Map<TourEntity>(item);

				var result = await _tourRepository.CreateAsync(tourEntity);

				if (result > 0)
				{
					_logger.Information("Tour created successfully, now creating schedules");

					var dateFrom = item.DateFrom;
					var dateTo = item.DateTo;

					while (dateFrom <= dateTo)
					{
						var dateStart = dateFrom;
						dateFrom = dateFrom.AddDays(item.DayList?.Count() ?? 0);
						var dateEnd = dateFrom;

                        if (dateEnd >= dateTo)
                        {
                            break;
                        }

						await _scheduleService.CreateAsync(new ScheduleRequestDTO
						{
							DateStart = dateStart,
							DateEnd = dateEnd,
							TourId = result,
							AvailableSeats = item.MaxGuests
						});

						dateFrom = dateFrom.AddDays(2);
					}

					_logger.Information("Schedules created successfully");
				}

				var createdSchedule = await _tourRepository.GetTourByIdAsync(result);
				var responseData = _mapper.Map<TourResponseDTO>(createdSchedule);

				_logger.Information("End: TourService - CreateAsync");

				return result > 0
					? new ApiResponse<TourResponseDTO>(200, responseData, "Tạo tour thành công")
					: new ApiResponse<TourResponseDTO>(400, null, "Tạo tour thất bại");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "An error occurred while creating the tour");

				return new ApiResponse<TourResponseDTO>(500, null, $"Lỗi khi tạo tour: {ex.Message}");
			}
		}

		public async Task<ApiResponse<TourResponseDTO>> UpdateAsync(int id, TourRequestDTO item)
        {
            _logger.Information($"Begin: TourService - UpdateAsync, id: {id}");

            var tour = await _tourRepository.GetTourByIdAsync(id);

            if (tour == null)
            {
                _logger.Information($"Tour not found, id: {id}");
                return new ApiResponse<TourResponseDTO>(404, null, "Không tìm thấy tour");
            }

			var existingReviews = tour.ReviewList;

			tour = _mapper.Map<TourEntity>(item);
			tour.Id = id;

			if (existingReviews != null)
			{
				tour.ReviewList = existingReviews;
			}

			tour.UpdatedAt = DateTime.UtcNow;

			var result = await _tourRepository.UpdateAsync(tour);
			_logger.Information($"result {result}");

			var updatedTour = await _tourRepository.GetTourByIdAsync(id);
			var responseData = _mapper.Map<TourResponseDTO>(updatedTour);

            _logger.Information("End: TourService - UpdateAsync");

            return result > 0
                ? new ApiResponse<TourResponseDTO>(200, responseData, "Cập nhật tour thành công")
                : new ApiResponse<TourResponseDTO>(400, null, "Cập nhật tour thất bại");
        }

        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin: TourService - DeleteAsync, id: {id}");
            var tour = await _tourRepository.GetTourByIdAsync(id);

            if (tour == null)
            {
                _logger.Information($"Tour not found, id: {id}");
                return new ApiResponse<int>(404, 0, "Không tìm thấy tour cần xóa");
            }

            await _tourRepository.SoftDeleteTourAsync(id);
            _logger.Information("End: TourService - DeleteAsync");

            return new ApiResponse<int>(200, id, "Xóa tour thành công (xóa giả)");
        }

        public async Task<ApiResponse<TourSearchResponseDTO>> SearchToursAsync(TourSearchRequestDTO searchRequest)
        {
            _logger.Information("Begin: TourService - SearchToursAsync");

            var data = await _tourRepository.SearchToursAsync(searchRequest);
            var tours = _mapper.Map<List<TourResponseDTO>>(data.Tours);
            var response = new TourSearchResponseDTO
            {
                Tours = tours,
                MaxPrice = data.MaxPrice,    
                MinPrice = data.MinPrice
			};
            _logger.Information("End: TourService - SearchToursAsync");
            return new ApiResponse<TourSearchResponseDTO>(200, response, "Tours retrieved successfully");
        }
    }
}

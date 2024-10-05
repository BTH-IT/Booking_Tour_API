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
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TourService(ITourRepository tourRepository, IMapper mapper, ILogger logger)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TourResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: TourService - GetAllAsync");
            var tours = await _tourRepository.FindAll().Where(h => h.DeletedAt == null)
                                                .ToListAsync();
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

        public async Task<ApiResponse<int>> CreateAsync(TourRequestDTO item)
        {
            _logger.Information("Begin: TourService - CreateAsync");

            var tourEntity = _mapper.Map<TourEntity>(item);
            await _tourRepository.CreateAsync(tourEntity);
            var result = await _tourRepository.SaveChangesAsync();
            _logger.Information("End: TourService - CreateAsync");
            return result > 0
                ? new ApiResponse<int>(200, tourEntity.Id, "Tạo tour thành công")
                : new ApiResponse<int>(400, 0, "Tạo tour thất bại");
        }


        public async Task<ApiResponse<TourResponseDTO>> UpdateAsync(TourRequestDTO item)
        {
            _logger.Information($"Begin: TourService - UpdateAsync, id: {item.Id}");

            var tour = await _tourRepository.GetTourByIdAsync(item.Id.Value);

            if (tour == null)
            {
                _logger.Information($"Tour not found, id: {item.Id}");
                return new ApiResponse<TourResponseDTO>(404, null, "Không tìm thấy tour");
            }

            tour = _mapper.Map(item, tour);

            var result = await _tourRepository.SaveChangesAsync();

            var responseData = _mapper.Map<TourResponseDTO>(tour);

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
        public async Task<ApiResponse<List<TourResponseDTO>>> SearchToursAsync(TourSearchRequestDTO searchRequest)
        {
            _logger.Information("Begin: TourService - SearchToursAsync");

            var tours = await _tourRepository.SearchToursAsync(searchRequest);
            var data = _mapper.Map<List<TourResponseDTO>>(tours);

            _logger.Information("End: TourService - SearchToursAsync");
            return new ApiResponse<List<TourResponseDTO>>(200, data, "Tours retrieved successfully");
        }
    }
}

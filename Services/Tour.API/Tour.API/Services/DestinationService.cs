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
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DestinationService(IDestinationRepository destinationRepository,
            IMapper mapper,
            ILogger logger)
        {
            _destinationRepository = destinationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DestinationResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: DestinationService - GetAllAsync");
            var destinations = await _destinationRepository.FindAll().ToListAsync();
            var data = _mapper.Map<List<DestinationResponseDTO>>(destinations);
            _logger.Information("End: DestinationService - GetAllAsync");
            return new ApiResponse<List<DestinationResponseDTO>>(200, data, "Lấy dữ liệu thành công");
        }

        public async Task<ApiResponse<DestinationResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin: DestinationService - GetByIdAsync, id: {id}");
            var destination = await _destinationRepository.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                _logger.Information($"Destination not found, id: {id}");
                return new ApiResponse<DestinationResponseDTO>(404, null, "Không tìm thấy điểm đến");
            }
            var data = _mapper.Map<DestinationResponseDTO>(destination);
            _logger.Information("End: DestinationService - GetByIdAsync");
            return new ApiResponse<DestinationResponseDTO>(200, data, "Lấy dữ liệu điểm đến thành công");
        }

        public async Task<ApiResponse<int>> CreateAsync(DestinationRequestDTO item)
        {
            _logger.Information("Begin: DestinationService - CreateAsync");
            var destinationEntity = _mapper.Map<DestinationEntity>(item);
            var newId = await _destinationRepository.CreateAsync(destinationEntity);
            _logger.Information("End: DestinationService - CreateAsync");
            return new ApiResponse<int>(200, newId, "Tạo điểm đến thành công");
        }

        public async Task<ApiResponse<DestinationResponseDTO>> UpdateAsync(DestinationRequestDTO item)
        {
            _logger.Information($"Begin: DestinationService - UpdateAsync, id: {item.Id}");
            var destination = await _destinationRepository.FindByCondition(d => d.Id == item.Id).FirstOrDefaultAsync();
            if (destination == null)
            {
                _logger.Information($"Destination not found, id: {item.Id}");
                return new ApiResponse<DestinationResponseDTO>(404, null, "Không tìm thấy điểm đến");
            }
            destination = _mapper.Map(item, destination);
            var result = await _destinationRepository.UpdateAsync(destination);
            _logger.Information("End: DestinationService - UpdateAsync");
            return result > 0
                ? new ApiResponse<DestinationResponseDTO>(200, _mapper.Map<DestinationResponseDTO>(destination), "Cập nhật thành công")
                : new ApiResponse<DestinationResponseDTO>(400, null, "Cập nhật thất bại");
        }

        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin: DestinationService - DeleteAsync, id: {id}");
            var destination = await _destinationRepository
                .FindByCondition(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (destination == null)
            {
                _logger.Information($"Destination not found, id: {id}");
                return new ApiResponse<int>(404, 0, "Không tìm thấy điểm đến cần xóa");
            }

            _destinationRepository.Delete(destination);
            var result = await _destinationRepository.SaveChangesAsync();
            _logger.Information("End: DestinationService - DeleteAsync");

            return result > 0
                ? new ApiResponse<int>(200, result, "Xóa điểm đến thành công")
                : new ApiResponse<int>(400, result, "Xóa điểm đến thất bại");
        }
    }
}

using AutoMapper;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
    public class BookingRoomService : IBookingRoomService
    {
        private readonly  IBookingRoomRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public BookingRoomService(IBookingRoomRepository repository,
            IMapper mapper,
            ILogger logger)
        {
            this._mapper = mapper;  
            this._logger = logger;
            this._repository = repository;
        }
        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin: BookingRoomService - DeleteAsync : {id}");

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                var bookingRoom = await _repository.GetBookingRoomByIdAsync(id);
                if (bookingRoom == null)
                {
                    return new ApiResponse<int>(404, 0, "Thông tin đặt phòng không được tìm thấy");
                }
                await _repository.DeleteBookingRoomAsync(bookingRoom.Id);

                _logger.Information($"End: BookingRoomService - DeleteAsync : {id}");
                return new ApiResponse<int>(200, 1, "Xóa thành công dữ liêu đặt phòng");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error($"Error during hotel deletion: {ex.Message}");
                return new ApiResponse<int>(500, 0, "An error occurred while deleting the hotel.");
            }
        }

        public Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync()
        {
            _logger.Information($"Begin: BookingRoomService - GetAllAsync ");
            var allData = _repository.GetBookingRoomsAsync();   

            
            _logger.Information($"Begin: BookingRoomService - GetAllAsync ");

        }

        public Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BookingRoomResponseDTO>> GetByUserIdAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BookingRoomResponseDTO>> UpdateAsync(HotelRequestDTO item)
        {
            throw new NotImplementedException();
        }
    }
}

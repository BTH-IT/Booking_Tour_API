using AutoMapper;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Room.API.Services
{
	public class HotelService : IHotelService
	{
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public HotelService(IHotelRepository hotelRepository,
			IMapper mapper,
			ILogger logger)
		{
			_hotelRepository = hotelRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<int>> CreateAsync(HotelRequestDTO item)
		{
			_logger.Information("Begin : HotelService - CreateAsync");

			var existingHotel = await _hotelRepository.GetHotelByNameAsync(item.Name);
			if (existingHotel != null)
			{
				return new ApiResponse<int>(400, -1, "Khách sạn đã tồn tại");
			}

			var hotelEntity = _mapper.Map<Hotel>(item);
			var newId = await _hotelRepository.CreateAsync(hotelEntity);

			_logger.Information("End : HotelService - CreateAsync");
			return new ApiResponse<int>(200, newId, "Tạo khách sạn thành công");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin : HotelService - DeleteAsync : {id}");

			var hotel = await _hotelRepository.GetHotelByIdAsync(id);
			if (hotel == null)
			{
				return new ApiResponse<int>(404, 0, "Không tìm thấy khách sạn cần xóa");
			}

			_hotelRepository.Delete(hotel);
			var result = await _hotelRepository.SaveChangesAsync();

			if (result > 0)
			{
				_logger.Information($"End : HotelService - DeleteAsync : {id} - Xóa thành công");
				return new ApiResponse<int>(200, result, "Xóa khách sạn thành công");
			}
			else
			{
				_logger.Information($"End : HotelService - DeleteAsync : {id} - Xóa thất bại");
				return new ApiResponse<int>(400, result, "Xóa khách sạn thất bại");
			}
		}

		public async Task<ApiResponse<List<HotelResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin : HotelService - GetAllAsync");

			var hotels = await _hotelRepository.FindAll(false, hotel => hotel.Rooms).ToListAsync();
			_logger.Information("Mapping list of hotels to DTO");

			var data = _mapper.Map<List<HotelResponseDTO>>(hotels);

			_logger.Information("End : HotelService - GetAllAsync");
			return new ApiResponse<List<HotelResponseDTO>>(200, data, "Lấy dữ liệu thành công");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin : HotelService - GetByIdAsync");

			var hotel = await _hotelRepository.GetHotelByIdAsync(id);
			if (hotel == null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Không tìm thấy khách sạn");
			}

			var data = _mapper.Map<HotelResponseDTO>(hotel);

			_logger.Information("End : HotelService - GetByIdAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Lấy dữ liệu khách sạn thành công");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information($"Begin : HotelService - GetByNameAsync");

			var hotel = await _hotelRepository.GetHotelByNameAsync(name);
			if (hotel == null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Không tìm thấy khách sạn");
			}

			var data = _mapper.Map<HotelResponseDTO>(hotel);

			_logger.Information("End : HotelService - GetByNameAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Lấy dữ liệu khách sạn thành công");
		}

		public async Task<ApiResponse<HotelResponseDTO>> UpdateAsync(HotelRequestDTO item)
		{
			_logger.Information("Begin : HotelService - UpdateAsync");

			var hotel = await _hotelRepository.GetHotelByIdAsync(item.Id);
			if (hotel == null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Không tìm thấy khách sạn");
			}

			if (await _hotelRepository.FindByCondition(h => h.Name.Equals(item.Name) && h.Id != item.Id).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<HotelResponseDTO>(400, null, "Tên khách sạn đã tồn tại");
			}

			hotel = _mapper.Map<Hotel>(item);
			var result = await _hotelRepository.UpdateAsync(hotel);

			if (result > 0)
			{
				_logger.Information("End : HotelService - UpdateAsync");
				return new ApiResponse<HotelResponseDTO>(200, _mapper.Map<HotelResponseDTO>(hotel), "Cập nhật thành công");
			}

			_logger.Information("End : HotelService - UpdateAsync");
			return new ApiResponse<HotelResponseDTO>(400, null, "Có lỗi xảy ra khi cập nhật");
		}
	}
}

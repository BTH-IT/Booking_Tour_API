using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Room.API.Services
{
	public class HotelService : IHotelService
	{
		private readonly IHotelRepository _hotelRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public HotelService(IHotelRepository hotelRepository, IRoomRepository roomRepository, IMapper mapper, ILogger logger)
		{
			_hotelRepository = hotelRepository;
			_roomRepository = roomRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<int>> CreateAsync(HotelRequestDTO item)
		{
			_logger.Information("Begin: HotelService - CreateAsync");

			var existingHotel = await _hotelRepository.GetHotelByNameAsync(item.Name);
			if (existingHotel != null)
			{
				return new ApiResponse<int>(400, -1, "Hotel already exists");
			}

			var hotelEntity = _mapper.Map<Hotel>(item);
			var newId = await _hotelRepository.CreateAsync(hotelEntity);

			_logger.Information("End: HotelService - CreateAsync");
			return new ApiResponse<int>(200, newId, "Hotel created successfully");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin : HotelService - DeleteAsync : {id}");

			using (var transaction = await _hotelRepository.BeginTransactionAsync())
			{
				try
				{
					var hotel = await _hotelRepository.GetHotelByIdAsync(id);
					if (hotel == null)
					{
						return new ApiResponse<int>(404, 0, "Hotel not found.");
					}

					hotel.DeletedAt = DateTime.UtcNow;
					_logger.Information("Checking _roomRepository is null: " + (_roomRepository == null));
					var rooms = await _roomRepository.FindByCondition(r => r.HotelId == id).ToListAsync();
					if (rooms != null || rooms.Any())
					{
						var bookedRooms = rooms.Where(r => r.IsAvailable).ToList();
						if (bookedRooms.Any())
						{
							return new ApiResponse<int>(400, 0, $"Rooms {string.Join(", ", bookedRooms.Select(r => r.Id))} are currently booked and cannot be deleted. All previous soft deletes have been reverted.");
						}

						foreach (var room in rooms)
						{
							room.DeletedAt = DateTime.UtcNow;
							await _roomRepository.UpdateRoomAsync(room);
						}
					}

					await _hotelRepository.UpdateAsync(hotel);
					await transaction.CommitAsync();

					_logger.Information($"End : HotelService - DeleteAsync : {id} - Successfully soft deleted the hotel and its rooms.");
					return new ApiResponse<int>(200, rooms.Count, "Successfully soft deleted the hotel and its associated rooms.");
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					_logger.Error($"Error during hotel deletion: {ex.Message}");
					return new ApiResponse<int>(500, 0, "An error occurred while deleting the hotel.");
				}
			}
		}

		public async Task<ApiResponse<List<HotelResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: HotelService - GetAllAsync");

			var hotels = await _hotelRepository.FindAll(false, hotel => hotel.Rooms)
												.Where(h => h.DeletedAt == null)
												.ToListAsync();

			_logger.Information("Mapping list of hotels to DTO");
			var data = _mapper.Map<List<HotelResponseDTO>>(hotels);

			_logger.Information("End: HotelService - GetAllAsync");
			return new ApiResponse<List<HotelResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: HotelService - GetByIdAsync");

			var hotel = await _hotelRepository.GetHotelByIdAsync(id);
			if (hotel == null || hotel.DeletedAt != null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}

			var data = _mapper.Map<HotelResponseDTO>(hotel);

			_logger.Information("End: HotelService - GetByIdAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: HotelService - GetByNameAsync");

			var hotel = await _hotelRepository.GetHotelByNameAsync(name);
			if (hotel == null || hotel.DeletedAt != null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}

			var data = _mapper.Map<HotelResponseDTO>(hotel);

			_logger.Information("End: HotelService - GetByNameAsync");
			return new ApiResponse<HotelResponseDTO>(200, data, "Hotel data retrieved successfully");
		}

		public async Task<ApiResponse<HotelResponseDTO>> UpdateAsync(HotelRequestDTO item)
		{
			_logger.Information("Begin: HotelService - UpdateAsync");

			var hotel = await _hotelRepository.GetHotelByIdAsync(item.Id);
			if (hotel == null || hotel.DeletedAt != null)
			{
				return new ApiResponse<HotelResponseDTO>(404, null, "Hotel not found");
			}

			if (await _hotelRepository.FindByCondition(h => h.Name.Equals(item.Name) && h.Id != item.Id && h.DeletedAt == null).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<HotelResponseDTO>(400, null, "Hotel name already exists");
			}

			hotel = _mapper.Map<Hotel>(item);
			var result = await _hotelRepository.UpdateAsync(hotel);

			if (result > 0)
			{
				_logger.Information("End: HotelService - UpdateAsync");
				return new ApiResponse<HotelResponseDTO>(200, _mapper.Map<HotelResponseDTO>(hotel), "Hotel updated successfully");
			}

			_logger.Information("End: HotelService - UpdateAsync");
			return new ApiResponse<HotelResponseDTO>(400, null, "Error occurred while updating hotel");
		}
	}
}

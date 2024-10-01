﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Room.API.Services
{
	public class RoomService : IRoomService
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper, ILogger logger)
		{
			_roomRepository = roomRepository;
			_hotelRepository = hotelRepository;
			_mapper = mapper;
			_logger = logger;
		}

		private HotelRulesResponseDTO MapHotelRules(string hotelRulesJson)
		{
			return new HotelRulesResponseDTO
			{
				HotelRules = JsonConvert.DeserializeObject<List<HotelRules>>(hotelRulesJson)?
							  .Select(rule => new HotelRulesDTO
							  {
								  Id = rule.Id,
								  Title = rule.Title
							  }).ToList()
			};
		}

		private HotelAmenitiesResponseDTO MapHotelAmenities(string hotelAmenitiesJson)
		{
			return new HotelAmenitiesResponseDTO
			{
				HotelAmenities = JsonConvert.DeserializeObject<List<HotelAmenities>>(hotelAmenitiesJson)?
								  .Select(amenity => new HotelAmenitiesDTO
								  {
									  Id = amenity.Id,
									  Title = amenity.Title
								  }).ToList()
			};
		}

		public async Task<ApiResponse<RoomResponseDTO>> CreateAsync(RoomRequestDTO item)
		{
			_logger.Information("Begin: RoomService - CreateAsync");

			var existingRoom = await _roomRepository.GetRoomByNameAsync(item.Name);
			if (existingRoom != null)
			{
				return new ApiResponse<RoomResponseDTO>(400, null, "Room already exists");
			}

			var roomEntity = _mapper.Map<RoomEntity>(item);
			var newId = await _roomRepository.CreateAsync(roomEntity);

			var createdRoom = await _roomRepository.GetRoomByIdAsync(newId);
			var responseData = _mapper.Map<RoomResponseDTO>(createdRoom);

			_logger.Information("End: RoomService - CreateAsync");
			return new ApiResponse<RoomResponseDTO>(200, responseData, "Room created successfully");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: RoomService - DeleteAsync: {id}");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null)
			{
				return new ApiResponse<int>(404, 0, "Room not found");
			}

			if (!room.IsAvailable)
			{
				return new ApiResponse<int>(400, 0, "Room is currently booked and cannot be deleted");
			}

			room.DeletedAt = DateTime.UtcNow;
			await _roomRepository.UpdateAsync(room);

			_logger.Information($"End: RoomService - DeleteAsync: {id} - Successfully deleted");
			return new ApiResponse<int>(200, id, "Room deleted successfully");
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: RoomService - GetAllAsync");

			var rooms = await _roomRepository.FindAll(false, room => room.Hotel)
											 .Where(r => r.DeletedAt == null)
											 .ToListAsync();

			var roomResponseList = new List<RoomResponseDTO>();

			foreach (var room in rooms)
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(room.HotelId);

				var roomResponse = _mapper.Map<RoomResponseDTO>(room);

				roomResponse.HotelRules = MapHotelRules(hotel.HotelRules);
				roomResponse.HotelAmenities = MapHotelAmenities(hotel.HotelAmenities);

				roomResponseList.Add(roomResponse);
			}

			_logger.Information("End: RoomService - GetAllAsync");

			return new ApiResponse<List<RoomResponseDTO>>(200, roomResponseList, "Data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: RoomService - GetByIdAsync");

			var room = await _roomRepository.GetRoomByIdAsync(id);
			if (room == null || room.DeletedAt != null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
			}

			var hotel = await _hotelRepository.GetHotelByIdAsync(room.HotelId);

			var roomResponse = _mapper.Map<RoomResponseDTO>(room);

			roomResponse.HotelRules = MapHotelRules(hotel.HotelRules);
			roomResponse.HotelAmenities = MapHotelAmenities(hotel.HotelAmenities);

			_logger.Information("End: RoomService - GetByIdAsync");
			return new ApiResponse<RoomResponseDTO>(200, roomResponse, "Room data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> GetByNameAsync(string name)
		{
			_logger.Information("Begin: RoomService - GetByNameAsync");

			var room = await _roomRepository.GetRoomByNameAsync(name);
			if (room == null || room.DeletedAt != null)
			{
				return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
			}

			var hotel = await _hotelRepository.GetHotelByIdAsync(room.HotelId);

			var roomResponse = _mapper.Map<RoomResponseDTO>(room);

			roomResponse.HotelRules = MapHotelRules(hotel.HotelRules);
			roomResponse.HotelAmenities = MapHotelAmenities(hotel.HotelAmenities);

			_logger.Information("End: RoomService - GetByNameAsync");
			return new ApiResponse<RoomResponseDTO>(200, roomResponse, "Room data retrieved successfully");
		}

		public async Task<ApiResponse<RoomResponseDTO>> UpdateAsync(RoomRequestDTO item)
		{
			try
			{
				_logger.Information("Begin: RoomService - UpdateAsync");

				var room = await _roomRepository.GetRoomByIdAsync(item.Id);
				if (room == null || room.DeletedAt != null)
				{
					return new ApiResponse<RoomResponseDTO>(404, null, "Room not found");
				}

				if (await _roomRepository.FindByCondition(r => r.Name.Equals(item.Name) && r.Id != item.Id && r.DeletedAt == null).FirstOrDefaultAsync() != null)
				{
					return new ApiResponse<RoomResponseDTO>(400, null, "Room name already exists");
				}

				room = _mapper.Map<RoomEntity>(item);
				var result = await _roomRepository.UpdateAsync(room);

				if (result > 0)
				{
					var updatedRoom = await _roomRepository.GetRoomByIdAsync(item.Id);
					var responseData = _mapper.Map<RoomResponseDTO>(updatedRoom);

					_logger.Information("End: RoomService - UpdateAsync");
					return new ApiResponse<RoomResponseDTO>(200, responseData, "Room updated successfully");
				}

				_logger.Information("End: RoomService - UpdateAsync");
				return new ApiResponse<RoomResponseDTO>(400, null, "Error occurred while updating room");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error occurred while updating room: {ex.Message}");
				return new ApiResponse<RoomResponseDTO>(500, null, "An unexpected error occurred while updating room.");
			}
		}

		public async Task<ApiResponse<List<RoomResponseDTO>>> SearchRoomsAsync(RoomSearchRequestDTO searchRequest)
		{
			_logger.Information("Begin: RoomService - SearchRoomsAsync");

			var rooms = await _roomRepository.SearchRoomsAsync(searchRequest);
			var roomResponseList = new List<RoomResponseDTO>();

			foreach (var room in rooms)
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(room.HotelId);

				var roomResponse = _mapper.Map<RoomResponseDTO>(room);

				roomResponse.HotelRules = MapHotelRules(hotel.HotelRules);
				roomResponse.HotelAmenities = MapHotelAmenities(hotel.HotelAmenities);

				roomResponseList.Add(roomResponse);
			}

			_logger.Information("End: RoomService - SearchRoomsAsync");
			return new ApiResponse<List<RoomResponseDTO>>(200, roomResponseList, "Rooms retrieved successfully");
		}
	}
}

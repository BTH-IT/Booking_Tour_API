using AutoMapper;
using Booking.API.GrpcClient.Protos;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
	public class BookingRoomService : IBookingRoomService
	{
		private readonly IBookingRoomRepository _bookingRoomRepository;
		private readonly IDetailBookingRoomRepository _detailBookingRoomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;	
		private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
		public BookingRoomService(IBookingRoomRepository bookingRoomRepository, 
			IDetailBookingRoomRepository detailBookingRoomRepository, 
			IMapper mapper, 
			ILogger logger,
			IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient,
			RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient)
		{
			_bookingRoomRepository = bookingRoomRepository;
			_detailBookingRoomRepository = detailBookingRoomRepository;
			_mapper = mapper;
			_logger = logger;
			_roomGrpcServiceClient = roomGrpcServiceClient;
			_identityGrpcServiceClient = identityGrpcServiceClient;
		}

		public async Task<ApiResponse<List<BookingRoomResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: BookingRoomService - GetAllAsync");

			try
			{
				var bookingRooms = await _bookingRoomRepository.GetBookingRoomsAsync();

				var data = _mapper.Map<List<BookingRoomResponseDTO>>(bookingRooms);
				foreach(var item in data)
				{
					await GetUserFromGrpcAsync(item);	
					await GetRoomsFromGrpcAsync(item);
				}	
				_logger.Information("End: BookingRoomService - GetAllAsync");
				return new ApiResponse<List<BookingRoomResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<BookingRoomResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<BookingRoomResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: BookingRoomService - GetByIdAsync: {id}");

			try
			{
				var bookingRoom = await _bookingRoomRepository.GetBookingRoomByIdAsync(id);

				if (bookingRoom == null)
				{
					_logger.Warning($"Booking room with ID {id} not found");
					return new ApiResponse<BookingRoomResponseDTO>(404, null, "Booking room not found");
				}

				var data = _mapper.Map<BookingRoomResponseDTO>(bookingRoom);
                await GetUserFromGrpcAsync(data);
                await GetRoomsFromGrpcAsync(data);

                _logger.Information($"End: BookingRoomService - GetByIdAsync: {id}");
				return new ApiResponse<BookingRoomResponseDTO>(200, data, "Booking room data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingRoomService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<BookingRoomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

        public async Task<ApiResponse<List<BookingRoomResponseDTO>>> GetCurrentUserAsync(int userId)
        {
            _logger.Information($"Begin: BookingRoomService - GetCurrentUserAsync: {userId}");

            try
            {
				var bookingRooms = await _bookingRoomRepository.FindByCondition(c=>c.UserId.Equals(userId),false,c=>c.DetailBookingRooms).ToListAsync();
				var bookingRoomDtos = _mapper.Map<List<BookingRoomResponseDTO>>(bookingRooms);
				foreach(var item in bookingRoomDtos)
				{
					await GetUserFromGrpcAsync(item);
                    await GetRoomsFromGrpcAsync(item);
                }
                _logger.Information($"End: BookingRoomService - GetCurrentUserAsync: {userId} - Successfully.");
                return new ApiResponse<List<BookingRoomResponseDTO>>(200, bookingRoomDtos, "Lấy dữ liệu thành công");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in BookingRoomService - DeleteAsync: {ex.Message}", ex);
                return new ApiResponse<List<BookingRoomResponseDTO>>(500, null, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

		private async Task GetUserFromGrpcAsync(BookingRoomResponseDTO dto)
		{
			_logger.Information($"START - BookingRoomService - GetUserFromGrpcAsync");
			try
			{
				var user = await _identityGrpcServiceClient.GetUserByIdAsync(new GetUserByIdRequest
				{
					Id = dto.UserId
				});
				dto.User = _mapper.Map<UserResponseDTO>(user);
                _logger.Information($"END - BookingRoomService - GetUserFromGrpcAsync");

            }
            catch (Exception ex)
			{
				_logger.Error($"{ex.Message}");

				_logger.Error("ERROR - BookingRoomService - GetUserFromGrpcAsync");
			}
		}

		private async Task GetRoomsFromGrpcAsync(BookingRoomResponseDTO dto)
		{
            _logger.Information($"START - BookingRoomService - GetRoomsFromGrpcAsync");
            try
            {
				var roomIds = dto.DetailBookingRooms.Select(c => c.RoomId);
				
				var request = new GetRoomsByIdsRequest();

				request.Ids.AddRange(roomIds);

				var roomInfos = await _roomGrpcServiceClient.GetRoomsByIdsAsync(request);

				var roomsDto = _mapper.Map<List<RoomResponseDTO>>(roomInfos.Rooms);

				var roomDictionary = roomsDto.ToDictionary(c => c.Id);

				foreach(var item in dto.DetailBookingRooms)
				{
					item.Room = roomDictionary[item.RoomId];	
				}	

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");

                _logger.Error("ERROR - BookingRoomService - GetRoomsFromGrpcAsync");
            }
        }

        public async Task<ApiResponse<RoomBookingDataDTO>> GetRoomCheckInCheckOutDataAsync(int roomId)
        {
			var bookingRooms = await _bookingRoomRepository.FindByCondition(c=>true,false,c=>c.DetailBookingRooms!.Where(tr => tr.DeletedAt == null)).ToListAsync();
			var roomBookingData = new RoomBookingDataDTO()
			{
				Data = new List<DetailRoomBookingDateDTO>()
			};
			foreach(var item in  bookingRooms)
			{
				if(item.DetailBookingRooms != null &&  item.DetailBookingRooms.Any(e=>e.RoomId.Equals(roomId)))
				{
					roomBookingData.Data.Add(new DetailRoomBookingDateDTO()
					{
						CheckIn = item.CheckIn!.Value,
						CheckOut = item.CheckOut!.Value
					});
				}
			}
			var response = new ApiResponse<RoomBookingDataDTO>(200,roomBookingData,"Lấy dữ liệu thành công");
			return response;
        }
    }
}

using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcClient.Protos;
using Booking.API.Repositories;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Grpc.Net.Client.Balancer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
	public class BookingTourService : IBookingTourService
	{
		private readonly IBookingTourRepository _bookingTourRepository;
		private readonly ITourBookingRoomRepository _tourBookingRoomRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;	
		private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
		public BookingTourService(IBookingTourRepository bookingTourRepository, 
			ITourBookingRoomRepository tourBookingRoomRepository, 
			IMapper mapper, 
			ILogger logger,
			IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient,
			TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient)
		{
			_bookingTourRepository = bookingTourRepository;
			_tourBookingRoomRepository = tourBookingRoomRepository;
			_mapper = mapper;
			_logger = logger;
			_identityGrpcServiceClient = identityGrpcServiceClient;	
			_tourGrpcServiceClient = tourGrpcServiceClient;
		}
		public async Task<ApiResponse<List<BookingTourResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: BookingTourService - GetAllAsync");

			try
			{
				var bookingTours = await _bookingTourRepository.GetBookingToursAsync();
				var data = _mapper.Map<List<BookingTourResponseDTO>>(bookingTours);

				foreach(var item in  data)
				{
					await GetUserFromGrpcAsync(item);
					await GetScheduleFromGrpcAsync(item);

				}
				_logger.Information("End: BookingTourService - GetAllAsync");
				return new ApiResponse<List<BookingTourResponseDTO>>(200, data, "Data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<BookingTourResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}
		public async Task<ApiResponse<BookingTourResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: BookingTourService - GetByIdAsync: {id}");

			try
			{
				var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(id);



                if (bookingTour == null)
				{
					_logger.Warning($"Booking tour with ID {id} not found");
					return new ApiResponse<BookingTourResponseDTO>(404, null, "Booking tour not found");
				}

				var data = _mapper.Map<BookingTourResponseDTO>(bookingTour);

                await GetUserFromGrpcAsync(data);
                await GetScheduleFromGrpcAsync(data);

                _logger.Information($"End: BookingTourService - GetByIdAsync: {id}");
				return new ApiResponse<BookingTourResponseDTO>(200, data, "Booking tour data retrieved successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in BookingTourService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<BookingTourResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}
        public async Task<ApiResponse<List<BookingTourResponseDTO>>> GetCurrentUserAsync(int userId)
        {
            _logger.Information($"START - BookingTourService - GetUserFromGrpcAsync");
            try
            {
                var bookingTours = await _bookingTourRepository.FindByCondition(c => c.UserId.Equals(userId), false, c => c.TourBookingRooms).ToListAsync();
                var bookingTourDtos = _mapper.Map<List<BookingTourResponseDTO>>(bookingTours);
                foreach (var item in bookingTourDtos)
                {
                    await GetUserFromGrpcAsync(item);
                    await GetScheduleFromGrpcAsync(item);
                }
                _logger.Information($"END - BookingTourService - GetUserFromGrpcAsync");
                return new ApiResponse<List<BookingTourResponseDTO>>(200, bookingTourDtos, "Lấy dữ liệu thành công");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                _logger.Error("ERROR - BookingTourService - GetUserFromGrpcAsync");
                return new ApiResponse<List<BookingTourResponseDTO>>(500, null, $"Có lỗi xảy ra: {ex.Message}");
            }
        }
        private async Task GetUserFromGrpcAsync(BookingTourResponseDTO dto)
        {
			_logger.Information($"START - BookingTourService - GetUserFromGrpcAsync");
			try
			{
				var user = await _identityGrpcServiceClient.GetUserByIdAsync(new GetUserByIdRequest
				{
					Id = dto.UserId
				});
				dto.User = _mapper.Map<UserResponseDTO>(user);
				_logger.Information($"END - BookingTourService - GetUserFromGrpcAsync");

			}
			catch (Exception ex)
			{
				_logger.Error($"{ex.Message}");

				_logger.Error("ERROR - BookingTourService - GetUserFromGrpcAsync");
			}
		}
		private async Task GetScheduleFromGrpcAsync(BookingTourResponseDTO item)
		{
            _logger.Information($"START - BookingTourService - GetScheduleFromGrpcAsync");
            try
            {
				var request = new GetScheduleByIdRequest { Id = item.Id };	
				var schedule = await _tourGrpcServiceClient.GetScheduleByIdAsync(request);

				item.Schedule = _mapper.Map<ScheduleResponseDTO>(schedule);

                _logger.Information($"END - BookingTourService - GetScheduleFromGrpcAsync");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");

                _logger.Error("ERROR - BookingRoomService - GetScheduleFromGrpcAsync");
            }
        }

        
    }
}

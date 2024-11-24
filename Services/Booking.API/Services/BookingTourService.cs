using AutoMapper;
using Booking.API.Entities;
using Booking.API.GrpcClient.Protos;
using Booking.API.Repositories;
using Booking.API.Repositories.Interfaces;
using Booking.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs;
using Shared.Enums;
using Shared.Helper;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using ILogger = Serilog.ILogger;

namespace Booking.API.Services
{
    public class BookingTourService : IBookingTourService
    {
        private readonly IBookingTourRepository _bookingTourRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public BookingTourService(IBookingTourRepository bookingTourRepository,
            IMapper mapper,
            ILogger logger,
            IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient,
            TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient,
            IPublishEndpoint publishEndpoint)
        {
            _bookingTourRepository = bookingTourRepository;
            _mapper = mapper;
            _logger = logger;
            _identityGrpcServiceClient = identityGrpcServiceClient;
            _tourGrpcServiceClient = tourGrpcServiceClient;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ApiResponse<List<BookingTourCustomResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: BookingTourService - GetAllAsync");

            try
            {
                var bookingTours = await _bookingTourRepository.GetBookingToursAsync();
                var data = _mapper.Map<List<BookingTourCustomResponseDTO>>(bookingTours);

                await GetUsersFromGrpcAsync(data);
                await GetAllInScheduleFromGrpcAsync(data);
                _logger.Information("End: BookingTourService - GetAllAsync");
                return new ApiResponse<List<BookingTourCustomResponseDTO>>(200, data, "Data retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in BookingTourService - GetAllAsync: {ex.Message}", ex);
                return new ApiResponse<List<BookingTourCustomResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ApiResponse<BookingTourCustomResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin: BookingTourService - GetByIdAsync: {id}");

            try
            {
                var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(id);



                if (bookingTour == null)
                {
                    _logger.Warning($"Booking tour with ID {id} not found");
                    return new ApiResponse<BookingTourCustomResponseDTO>(404, null, "Booking tour not found");
                }

                var data = _mapper.Map<BookingTourCustomResponseDTO>(bookingTour);

                await GetUserFromGrpcAsync(data);
                await GetScheduleFromGrpcAsync(data);

                _logger.Information($"End: BookingTourService - GetByIdAsync: {id}");
                return new ApiResponse<BookingTourCustomResponseDTO>(200, data, "Booking tour data retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in BookingTourService - GetByIdAsync: {ex.Message}", ex);
                return new ApiResponse<BookingTourCustomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ApiResponse<BookingTourCustomResponseDTO>> UpdateBookingTourInfoAsync(int bookingTourId, UpdateBookingTourInfoRequest request, int userId, int role)
        {
            try
            {
                var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(bookingTourId);

                if (bookingTour == null)
                {
                    _logger.Warning($"Booking tour with ID {bookingTourId} not found");
                    return new ApiResponse<BookingTourCustomResponseDTO>(404, null, "Booking tour not found");
                }

                if (bookingTour.UserId != userId && role != (int)ERole.Admin)
                {
                    _logger.Warning($"Bad request");
                    return new ApiResponse<BookingTourCustomResponseDTO>(403, null, "Not allowed to change this booking tour");
                }

                bookingTour.TravellerList = _mapper.Map<List<Traveller>>(request.Travellers);

                _bookingTourRepository.Update(bookingTour);
                _logger.Information($"End: BookingTourService - UpdateBookingTourInfoAsync: {bookingTourId}");

                var data = await GetByIdAsync(bookingTourId);
                return new ApiResponse<BookingTourCustomResponseDTO>(200, data.Result, "Booking tour data retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in BookingTourService - UpdateBookingTourInfoAsync: {ex.Message}", ex);
                return new ApiResponse<BookingTourCustomResponseDTO>(500, null, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<BookingTourCustomResponseDTO>>> GetCurrentUserAsync(int userId)
        {
            _logger.Information($"START - BookingTourService - GetUserFromGrpcAsync");
            try
            {
                var bookingTours = await _bookingTourRepository.FindByCondition(c => c.UserId.Equals(userId), false).ToListAsync();
                var bookingTourDtos = _mapper.Map<List<BookingTourCustomResponseDTO>>(bookingTours);
                await GetUsersFromGrpcAsync(bookingTourDtos);
                await GetAllInScheduleFromGrpcAsync(bookingTourDtos);
                _logger.Information($"END - BookingTourService - GetUserFromGrpcAsync");
                return new ApiResponse<List<BookingTourCustomResponseDTO>>(200, bookingTourDtos, "Lấy dữ liệu thành công");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                _logger.Error("ERROR - BookingTourService - GetUserFromGrpcAsync");
                return new ApiResponse<List<BookingTourCustomResponseDTO>>(500, null, $"Có lỗi xảy ra: {ex.Message}");
            }
        }
        private async Task GetUserFromGrpcAsync(BookingTourCustomResponseDTO dto)
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
        private async Task GetUsersFromGrpcAsync(List<BookingTourCustomResponseDTO> dto)
        {
            _logger.Information($"START - BookingTourService - GetUsersFromGrpcAsync");
            try
            {
                var userIds = dto.Select(c => c.UserId).ToList();

                var getUsersRequest = new GetUsersByIdRequest();

                getUsersRequest.Ids.AddRange(userIds.Distinct().ToList());

                var userInfos = await _identityGrpcServiceClient.GetUsersByIdsAsync(getUsersRequest);

                var userDtos = _mapper.Map<List<UserResponseDTO>>(userInfos.Users);

                var userDictionary = userDtos.ToDictionary(c => c.Id);

                foreach (var item in dto)
                {
                    item.User = userDictionary.ContainsKey(item.UserId) ? userDictionary[item.UserId] : null;
                }
                _logger.Information($"END - BookingTourService - GetUsersFromGrpcAsync");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");

                _logger.Error("ERROR - BookingRoomService - GetUsersFromGrpcAsync");
            }
        }
        private async Task GetAllInScheduleFromGrpcAsync(List<BookingTourCustomResponseDTO> dto)
        {
            _logger.Information($"START - BookingTourService - GetAllInScheduleFromGrpcAsync");
            try
            {
                var scheduleIds = dto.Select(c => c.ScheduleId).ToList();

                var getSchedulesRequest = new GetSchedulesByIdsRequest();

                getSchedulesRequest.Ids.AddRange(scheduleIds.Distinct().ToList());

                var scheduleInfos = await _tourGrpcServiceClient.GetSchedulesByIdsAsync(getSchedulesRequest);

                var scheduleDtos = _mapper.Map<List<ScheduleCustomResponseDTO>>(scheduleInfos.Schedules);

                var scheduleDictionary = scheduleDtos.ToDictionary(c => c.Id);

                foreach (var item in dto)
                {
                    item.Schedule = scheduleDictionary.ContainsKey(item.ScheduleId) ? scheduleDictionary[item.ScheduleId] : null;
                }
                _logger.Information($"END - BookingTourService - GetAllInScheduleFromGrpcAsync");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");

                _logger.Error("ERROR - BookingRoomService - GetAllInScheduleFromGrpcAsync");
            }
        }
        private async Task GetScheduleFromGrpcAsync(BookingTourCustomResponseDTO item)
        {
            _logger.Information($"START - BookingTourService - GetScheduleFromGrpcAsync");
            try
            {
                var request = new GetScheduleByIdRequest { Id = item.Id };
                var response = await _tourGrpcServiceClient.GetScheduleByIdAsync(request);

                item.Schedule = _mapper.Map<ScheduleCustomResponseDTO>(response.Schedule);

                _logger.Information($"END - BookingTourService - GetScheduleFromGrpcAsync");

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");

                _logger.Error("ERROR - BookingRoomService - GetScheduleFromGrpcAsync");
            }
        }
        public async Task<ApiResponse<string>> DeleteBookingTourAsync(int bookingTourId, int userId)
        {
            _logger.Information($"START - BookingTourService - DeleteBookingTourAsync");

            var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(bookingTourId);

            if (bookingTour.UserId != userId)
            {
                return new ApiResponse<string>(400, "", "Chỉ có chủ của đơn đặt mới có thể hủy");
            }
            if (bookingTour.Status == Constants.OrderStatus.Done)
            {
                return new ApiResponse<string>(400, "", "Đơn đặt đã thanh toán không thể hủy");
            }
            var now = DateTime.Now;
            TimeSpan timeDifference = now - bookingTour.CreatedAt;
            if (timeDifference.TotalHours >= 24)
            {
                return new ApiResponse<string>(400, "", "Đã qua 24 tiếng nên không thể hủy");
            }

            bookingTour.Status = Constants.OrderStatus.Cancelled;
            var result = await _bookingTourRepository.UpdateAsync(bookingTour);
            if (result > 0)
            {
                var request = new UpdateScheduleAvailableSeatRequest
                {
                    ScheduleId = bookingTour.ScheduleId,
                    Count = bookingTour.Seats * -1
                };
                await _tourGrpcServiceClient.UpdateScheduleAvailableSeatAsync(request);

                await _publishEndpoint.Publish(new BookingTourEvent
                {
                    Id = Guid.NewGuid(),
                    Data = _mapper.Map<BookingTourCustomResponseDTO>(bookingTour),
                    Type = "CANCEL"
                });
            }
            _logger.Information($"END - BookingTourService - DeleteBookingTourAsync");

            return new ApiResponse<string>(200, "", "Hủy đơn đặt thành công");
        }

        public async Task<ApiResponse<string>> UpdateStatusBookingTourAsync(int bookingTourId, UpdateBookingStatusDTO dto)
        {
            var bookingTour = await _bookingTourRepository.GetBookingTourByIdAsync(bookingTourId);
            if (bookingTour == null)
            {
                return new ApiResponse<string>(404, "", "Không tìm thấy đơn đặt");
            }
            if (bookingTour.Status.Equals(Constants.OrderStatus.Done))
            {
                return new ApiResponse<string>(400, "", "Đơn đã thanh toán không thể thay đổi trạng thái");

            }
            bookingTour.Status = dto.Status;
            await _bookingTourRepository.SaveChangesAsync();

            await _publishEndpoint.Publish(new BookingTourEvent
            {
                Id = Guid.NewGuid(),
                Data = _mapper.Map<BookingTourCustomResponseDTO>(bookingTour),
                Type = "STATUS_UPDATE"
            });
            _logger.Information($"END - BookingTourService - UpdateStatusBookingTourAsync");
            return new ApiResponse<string>(400, "", "Cập nhật thành công");

        }
    }
}

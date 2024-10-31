using AutoMapper;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Entities;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services.Interfaces;
using Tour.API.GrpcClient.Protos;
using ILogger = Serilog.ILogger;
using Grpc.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tour.API.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourRoomRepository _tourRoomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IScheduleService _scheduleService;
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;

        public TourService(ITourRepository tourRepository, 
            ITourRoomRepository tourRoomRepository,
            IScheduleService scheduleService, 
            RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient,
            IMapper mapper,
            ILogger logger)
        {
            _tourRepository = tourRepository;
            _tourRoomRepository = tourRoomRepository;
            _scheduleService = scheduleService;
            _roomGrpcServiceClient = roomGrpcServiceClient;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task GetRoomsFromGrpcAsync(TourResponseDTO dto)
        {
            _logger.Information($"START - BookingRoomService - GetRoomsFromGrpcAsync");
            try
            {
                var roomIds = dto.TourRooms.Select(c => c.RoomId);

                var request = new GetRoomsByIdsRequest();

                request.Ids.AddRange(roomIds);

                var roomInfos = await _roomGrpcServiceClient.GetRoomsByIdsAsync(request);

                var roomsDto = _mapper.Map<List<RoomResponseDTO>>(roomInfos.Rooms);

                var roomDictionary = roomsDto.ToDictionary(c => c.Id);

                foreach (var item in dto.TourRooms)
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

        public async Task<ApiResponse<List<TourResponseDTO>>> GetAllAsync()
        {
            _logger.Information("Begin: GetAllAsync");
            try
            {
                var tours = await _tourRepository.GetToursAsync();
                var tourDtos = _mapper.Map<List<TourResponseDTO>>(tours);

                    foreach (var item in tourDtos)
                    {
					    await GetRoomsFromGrpcAsync(item);
                    }

                return new ApiResponse<List<TourResponseDTO>>(200, tourDtos, "Successfully retrieved the list of tours");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving the list of tours");
                return new ApiResponse<List<TourResponseDTO>>(500, null, "An error occurred while retrieving the list of tours");
            }
            finally
            {
                _logger.Information("End: GetAllAsync");
            }
        }

        public async Task<ApiResponse<TourResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin: GetByIdAsync, id: {id}");
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(id);
                if (tour == null) return NotFound<TourResponseDTO>(id);

                var data = _mapper.Map<TourResponseDTO>(tour);

                await GetRoomsFromGrpcAsync(data);

                return new ApiResponse<TourResponseDTO>(200, data, "Successfully retrieved tour data");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving the tour");
                return new ApiResponse<TourResponseDTO>(500, null, $"An error occurred while retrieving the tour: {ex.Message}");
            }
            finally
            {
                _logger.Information("End: GetByIdAsync");
            }
        }

        public async Task<ApiResponse<TourResponseDTO>> CreateAsync(TourRequestDTO item)
        {
            _logger.Information("Begin: CreateAsync");
            try
            {
                var tourEntity = _mapper.Map<TourEntity>(item);
                var result = await _tourRepository.CreateTourAsync(tourEntity);

                if (result > 0)
                {
                    await CreateSchedulesAsync(item, result);
                    var createdTour = await _tourRepository.GetTourByIdAsync(result);
                    var responseData = _mapper.Map<TourResponseDTO>(createdTour);
                    return new ApiResponse<TourResponseDTO>(200, responseData, "Tour created successfully");
                }

                return new ApiResponse<TourResponseDTO>(400, null, "Failed to create tour");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while creating the tour");
                return new ApiResponse<TourResponseDTO>(500, null, $"Error while creating tour: {ex.Message}");
            }
            finally
            {
                _logger.Information("End: CreateAsync");
            }
        }

        private async Task CreateSchedulesAsync(TourRequestDTO item, int tourId)
        {
            var dateFrom = item.DateFrom;
            var dateTo = item.DateTo;

            while (dateFrom <= dateTo)
            {
                var dateStart = dateFrom;
                dateFrom = dateFrom.AddDays(item.DayList?.Count() ?? 0);
                var dateEnd = dateFrom;

                if (dateEnd >= dateTo) break;

                await _scheduleService.CreateAsync(new ScheduleRequestDTO
                {
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    TourId = tourId,
                    AvailableSeats = item.MaxGuests
                });

                dateFrom = dateFrom.AddDays(2);
            }
        }

        public async Task<ApiResponse<TourResponseDTO>> UpdateAsync(int id, TourRequestDTO item)
        {
            _logger.Information($"Begin: UpdateAsync, id: {id}");
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(id);
                if (tour == null) return NotFound<TourResponseDTO>(id);

                _mapper.Map(item, tour);
                tour.UpdatedAt = DateTime.UtcNow;

                var result = await _tourRepository.UpdateTourAsync(tour);
                await UpdateTourRoomsAsync(id, item.TourRooms);

                var updatedTour = await _tourRepository.GetTourByIdAsync(id);
                var responseData = _mapper.Map<TourResponseDTO>(updatedTour);
                return new ApiResponse<TourResponseDTO>(result > 0 ? 200 : 400, responseData, result > 0 ? "Successfully updated the tour" : "Failed to update the tour");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating the tour");
                return new ApiResponse<TourResponseDTO>(500, null, $"An error occurred while updating the tour: {ex.Message}");
            }
            finally
            {
                _logger.Information("End: UpdateAsync");
            }
        }

        private async Task UpdateTourRoomsAsync(int tourId, List<TourRoomRequestDTO> newTourRooms)
        {
            var existingTourRooms = await _tourRoomRepository.GetTourRoomsByTourIdAsync(tourId);
            var existingRoomIds = existingTourRooms.Select(tr => tr.RoomId).ToHashSet();

            foreach (var tourRoom in existingTourRooms.Where(tr => !newTourRooms.Any(nr => nr.RoomId == tr.RoomId)))
            {
                await _tourRoomRepository.DeleteTourRoomAsync(tourRoom.Id);
            }

            foreach (var newTourRoom in newTourRooms)
            {
                if (!existingRoomIds.Contains(newTourRoom.RoomId))
                {
                    await _tourRoomRepository.CreateTourRoomAsync(new TourRoom
                    {
                        TourId = tourId,
                        RoomId = newTourRoom.RoomId,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin: DeleteAsync, id: {id}");
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(id);
                if (tour == null) return NotFound<int>(id);

                await DeleteTourRoomsAsync(id);
                await _tourRepository.DeleteTourAsync(id);

                return new ApiResponse<int>(200, id, "Successfully deleted the tour (soft delete)");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while deleting the tour");
                return new ApiResponse<int>(500, 0, $"An error occurred while deleting the tour: {ex.Message}");
            }
            finally
            {
                _logger.Information("End: DeleteAsync");
            }
        }

        private async Task DeleteTourRoomsAsync(int tourId)
        {
            var tourRooms = await _tourRoomRepository.GetTourRoomsByTourIdAsync(tourId);
            foreach (var tourRoom in tourRooms)
            {
                await _tourRoomRepository.DeleteTourRoomAsync(tourRoom.Id);
            }
        }

        public async Task<ApiResponse<TourSearchResponseDTO>> SearchToursAsync(TourSearchRequestDTO searchRequest)
        {
            _logger.Information("Begin: SearchToursAsync");
            try
            {
                var data = await _tourRepository.SearchToursAsync(searchRequest);
                var tours = _mapper.Map<List<TourResponseDTO>>(data.Tours);
                var response = new TourSearchResponseDTO
                {
                    Tours = tours,
                    MaxPrice = data.MaxPrice,
                    MinPrice = data.MinPrice
                };
                return new ApiResponse<TourSearchResponseDTO>(200, response, "Tours retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while searching for tours");
                return new ApiResponse<TourSearchResponseDTO>(500, null, $"An error occurred while searching for tours: {ex.Message}");
            }
            finally
            {
                _logger.Information("End: SearchToursAsync");
            }
        }

        private ApiResponse<T> NotFound<T>(int id) =>
            new ApiResponse<T>(404, default, $"No tour found with id: {id}");
    }
}
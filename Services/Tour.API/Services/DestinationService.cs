using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Entities;
using Tour.API.Repositories.Interfaces;
using Tour.API.Services.Interfaces;
using ILogger = Serilog.ILogger;
using EventBus.IntergrationEvents.Events;
using MassTransit;

namespace Tour.API.Services
{
	public class DestinationService : IDestinationService
	{
		private readonly IDestinationRepository _destinationRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IPublishEndpoint _publishEndpoint;

		public DestinationService(IDestinationRepository destinationRepository,
		IMapper mapper,
		ILogger logger,
		IPublishEndpoint publishEndpoint)
		{
			_destinationRepository = destinationRepository;
			_mapper = mapper;
			_logger = logger;
			_publishEndpoint = publishEndpoint;
		}

		public async Task<ApiResponse<List<DestinationResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: DestinationService - GetAllAsync");
			try
			{
				var destinations = await _destinationRepository.GetDestinationsAsync();
				var data = _mapper.Map<List<DestinationResponseDTO>>(destinations);
				_logger.Information("End: DestinationService - GetAllAsync");
				return new ApiResponse<List<DestinationResponseDTO>>(200, data, "Data retrieved successfully.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in DestinationService - GetAllAsync: {ex.Message}", ex);
				return new ApiResponse<List<DestinationResponseDTO>>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<DestinationResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin: DestinationService - GetByIdAsync, id: {id}");
			try
			{
				var destination = await _destinationRepository.GetDestinationByIdAsync(id);
				if (destination == null)
				{
					_logger.Information($"Destination not found, id: {id}");
					return new ApiResponse<DestinationResponseDTO>(404, null, "Destination not found.");
				}
				var data = _mapper.Map<DestinationResponseDTO>(destination);
				_logger.Information("End: DestinationService - GetByIdAsync");
				return new ApiResponse<DestinationResponseDTO>(200, data, "Destination data retrieved successfully.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in DestinationService - GetByIdAsync: {ex.Message}", ex);
				return new ApiResponse<DestinationResponseDTO>(500, null, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<ApiResponse<DestinationResponseDTO>> CreateAsync(DestinationRequestDTO item)
		{
			_logger.Information("Begin: DestinationService - CreateAsync");
			try
			{
				var destinationEntity = _mapper.Map<DestinationEntity>(item);
				var newId = await _destinationRepository.CreateDestinationAsync(destinationEntity);

				if (newId <= 0)
				{
					_logger.Warning("Failed to create destination");
					return new ApiResponse<DestinationResponseDTO>(400, null, "Error occurred while creating the destination.");
				}

				var createdDestination = await _destinationRepository.GetDestinationByIdAsync(newId);
				var responseData = _mapper.Map<DestinationResponseDTO>(createdDestination);
				await _publishEndpoint.Publish(new DestinationEvent
				{
					Id = Guid.NewGuid(),
					Data = responseData,
					Type = "CREATE"
				});

				_logger.Information("End: DestinationService - CreateAsync");
				return new ApiResponse<DestinationResponseDTO>(200, responseData, "Destination created successfully.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in DestinationService - CreateAsync: {ex.Message}", ex);
				return new ApiResponse<DestinationResponseDTO>(500, null, $"An error occurred while creating the destination: {ex.Message}");
			}
		}

		public async Task<ApiResponse<DestinationResponseDTO>> UpdateAsync(int id, DestinationRequestDTO item)
		{
			_logger.Information($"Begin: DestinationService - UpdateAsync, id: {id}");
			try
			{
				var destination = await _destinationRepository.FindByCondition(d => d.Id == id).FirstOrDefaultAsync();
				if (destination == null)
				{
					_logger.Information($"Destination not found, id: {id}");
					return new ApiResponse<DestinationResponseDTO>(404, null, "Destination not found.");
				}

				_mapper.Map(item, destination);
				destination.UpdatedAt = DateTime.UtcNow;

				var result = await _destinationRepository.UpdateDestinationAsync(destination);

				if (result <= 0)
				{
					_logger.Warning("Failed to update destination");
					return new ApiResponse<DestinationResponseDTO>(400, null, "Error occurred while updating the destination.");
				}

				var updatedDestination = await _destinationRepository.GetDestinationByIdAsync(id);
				var responseData = _mapper.Map<DestinationResponseDTO>(updatedDestination);
				await _publishEndpoint.Publish(new DestinationEvent
				{
					Id = Guid.NewGuid(),
					Data = responseData,
					Type = "UPDATE"
				});

				_logger.Information("End: DestinationService - UpdateAsync");
				return result > 0
					? new ApiResponse<DestinationResponseDTO>(200, responseData, "Update successful.")
					: new ApiResponse<DestinationResponseDTO>(400, null, "Update failed.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in DestinationService - UpdateAsync: {ex.Message}", ex);
				return new ApiResponse<DestinationResponseDTO>(500, null, $"An error occurred while updating the destination: {ex.Message}");
			}
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: DestinationService - DeleteAsync, id: {id}");
			try
			{
				var destination = await _destinationRepository
					.FindByCondition(d => d.Id == id)
					.FirstOrDefaultAsync();

				if (destination == null)
				{
					_logger.Information($"Destination not found, id: {id}");
					return new ApiResponse<int>(404, 0, "Destination to delete not found.");
				}

				_destinationRepository.Delete(destination);
				var result = await _destinationRepository.SaveChangesAsync();
				_logger.Information("End: DestinationService - DeleteAsync");
				var responseData = _mapper.Map<DestinationResponseDTO>(result);

				if (result > 0)
				{
					await _publishEndpoint.Publish(new DestinationEvent
					{
						Id = Guid.NewGuid(),
						Data = responseData,
						Type = "DELETE"
					});
				}

				return result > 0
					? new ApiResponse<int>(200, result, "Destination deleted successfully.")
					: new ApiResponse<int>(400, result, "Failed to delete destination.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Error in DestinationService - DeleteAsync: {ex.Message}", ex);
				return new ApiResponse<int>(500, 0, $"An error occurred while deleting the destination: {ex.Message}");
			}
		}
	}
}

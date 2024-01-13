using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApi.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;

        public DestinationService(IDestinationRepository destinationRepository, IMapper mapper)
        {
            _destinationRepository = destinationRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _destinationRepository.Delete(id);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = id;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<List<DestinationResponseDTO>> GetAll()
        {
            try
            {
                var destinations = await _destinationRepository.GetAll();
                var responseDTOs = _mapper.Map<List<Destination>, List<DestinationResponseDTO>>(destinations);

                return responseDTOs;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<DestinationResponseDTO> GetById(int id)
        {
            try
            {
                var destination = await _destinationRepository.GetById(id);
                var responseDTO = _mapper.Map<Destination, DestinationResponseDTO>(destination);

                return responseDTO;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<APIResponse<int>> Insert(DestinationRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _destinationRepository.Insert(item);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = insertedItemId;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<DestinationResponseDTO> Update(DestinationRequestDTO item)
        {
            try
            {
                var updatedDestination = await _destinationRepository.Update(item);
                var responseDTO = _mapper.Map<Destination, DestinationResponseDTO>(updatedDestination);

                return responseDTO;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }
    }
}

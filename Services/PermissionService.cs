using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Models;
using BookingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApi.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _permissionRepository.Delete(id);
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

        public async Task<List<PermissionResponseDTO>> GetAll()
        {
            try
            {
                var permissions = await _permissionRepository.GetAll();
                var responseDTOs = _mapper.Map<List<Permission>, List<PermissionResponseDTO>>(permissions);

                return responseDTOs;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<PermissionResponseDTO> GetById(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetById(id);
                var responseDTO = _mapper.Map<Permission, PermissionResponseDTO>(permission);

                return responseDTO;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<APIResponse<int>> Insert(PermissionRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _permissionRepository.Insert(item);
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

        public async Task<PermissionResponseDTO> Update(PermissionRequestDTO item)
        {
            try
            {
                var updatedPermission = await _permissionRepository.Update(item);
                var responseDTO = _mapper.Map<Permission, PermissionResponseDTO>(updatedPermission);

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

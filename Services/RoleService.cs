using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Models;
using BookingApi.Repositories;
using BookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly string[] _actionList = new [] {"CREATE", "READ", "UPDATE", "DELETE"};
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _roleRepository.Delete(id);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = id;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Not Found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public Task<List<RoleResponseDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponseDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoleDetailDTO>> GetRoleDetailAllByRoleId(int id)
        {
            var result = await _roleRepository.GetRoleDetailAllByRoleId(id);

            if (result.Count > 0) return new List<RoleDetailDTO>();

            List<RoleDetailDTO> roleDetailDTOs = _mapper.Map<List<RoleDetail>, List<RoleDetailDTO>>(result);

            return roleDetailDTOs;
        }

        public async Task<APIResponse<int>> Insert(RoleRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _roleRepository.Insert(item);
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

        public async Task<RoleResponseDTO> Update(RoleRequestDTO item)
        {
            Role data = await _roleRepository.Update(item);

            if (data == null) return null;

            RoleResponseDTO role = _mapper.Map<Role, RoleResponseDTO>(data);

            return role;
        }

        public async Task<RoleDetailDTO> UpdateRoleDetailByRoleId(RoleDetailDTO item)
        {
            RoleDetail data = await _roleRepository.UpdateRoleDetailByRoleId(item);

            if (data == null) return null;

            RoleDetailDTO roleDetail = _mapper.Map<RoleDetail, RoleDetailDTO>(data);

            return roleDetail;
        }
    }
}

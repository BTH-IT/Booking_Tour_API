using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Models;
using BookingApi.Services.Interfaces;

namespace BookingApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _userRepository.Delete(id);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = id;
                } else
                {
                    response.StatusCode = 404;
                    response.Result = id;
                    response.Message = "Not Found";
                }
            } catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<List<UserResponseDTO>> GetAll()
        {
            List<User> data = await _userRepository.GetAll();

            if (data == null) return null;

            List<UserResponseDTO> users = _mapper.Map<List<User>, List<UserResponseDTO>>(data);

            return users;
        }

        public async Task<UserResponseDTO> GetById(int id)
        {
            User data = await _userRepository.GetById(id);

            if (data == null) return null;

            UserResponseDTO user = _mapper.Map<User, UserResponseDTO>(data);
            
            return user;
        }

        public async Task<APIResponse<int>> Insert(UserRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _userRepository.Insert(item);
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

        public async Task<UserResponseDTO> Update(UserRequestDTO item)
        {
            User data = await _userRepository.Update(item);

            if (data == null) return null;

            UserResponseDTO user = _mapper.Map<User, UserResponseDTO>(data);

            return user;
        }
    }
}

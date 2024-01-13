
using BookingApi.DTO;
using BookingApi.Helpers;

namespace BookingApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDTO>> GetAll();
        Task<UserResponseDTO> GetById(int id);
        Task<APIResponse<int>> Insert(UserRequestDTO item);
        Task<UserResponseDTO> Update(UserRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}

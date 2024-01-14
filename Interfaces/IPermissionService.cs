using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionResponseDTO>> GetAll();
        Task<PermissionResponseDTO> GetById(int id);
        Task<APIResponse<int>> Insert(PermissionRequestDTO item);
        Task<PermissionResponseDTO> Update(PermissionRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}

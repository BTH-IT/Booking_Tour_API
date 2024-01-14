using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAll();
        Task<Permission> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(PermissionRequestDTO item);
        Task<Permission> Update(PermissionRequestDTO item);
        Task<bool> Delete(int id);
    }
}

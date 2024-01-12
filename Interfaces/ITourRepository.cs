using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Interfaces
{
    public interface ITourRepository
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(RoleRequestDTO item);
        Task<Role> Update(RoleRequestDTO item);
        Task<bool> Delete(int id);
    }
}

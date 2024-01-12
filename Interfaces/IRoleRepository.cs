using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(int id);
        Task<List<RoleDetail>> GetRoleDetailAllByRoleId(int id);
        Task<RoleDetail> UpdateRoleDetailByRoleId(RoleDetailDTO item);
        Task<(bool isSuccess, int insertedItemId)> Insert(RoleRequestDTO item);
        Task<Role> Update(RoleRequestDTO item);
        Task<bool> Delete(int id);
    }
}

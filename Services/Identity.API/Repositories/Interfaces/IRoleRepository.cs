using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;

namespace Identity.API.Repositories.Interfaces
{
    public interface IRoleRepository : IRepositoryBase<Role,int,IdentityDbContext>
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
        Task<RoleDetail> UpdateRoleDetailByRoleIdAsync(int id,RoleDetail roleDetail);
        Task DeleteRoleDetailByRoleIdAsync(int roleId);
    }
}

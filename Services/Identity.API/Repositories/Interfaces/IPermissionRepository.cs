using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;

namespace Identity.API.Repositories.Interfaces
{
    public interface IPermissionRepository : IRepositoryBase<Permission,int,IdentityDbContext>
    {
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(int id);
        Task<Permission> GetPermissionByNameAsync(string name);
        Task CreatePermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task DeletePermissionAsync(int id);
    }
}

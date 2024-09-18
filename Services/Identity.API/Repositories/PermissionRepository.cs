using Identity.API.Persistence;
using Infrastructure.Repositories;
using Identity.API.Entites;
using Identity.API.Repositories.Interfaces;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repositories
{
    public class PermissionRepository : RepositoryBase<Permission, int, IdentityDbContext>, IPermissionRepository
    {
        public PermissionRepository(IdentityDbContext dbContext, IUnitOfWork<IdentityDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreatePermissionAsync(Permission permission) => CreateAsync(permission);

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await GetPermissionByIdAsync(id);
            if(permission != null)
            {
                await DeleteAsync(permission);
            }
        }
        public Task<Permission> GetPermissionByIdAsync(int id) => FindByCondition(c=>c.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<Permission>> GetPermissionsAsync() => await FindAll().ToListAsync();

        public Task UpdatePermissionAsync(Permission permission) => UpdateAsync(permission);
    }
}

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
				permission.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(permission);
			}
		}

        public Task<Permission> GetPermissionByIdAsync(int id) => FindByCondition(c=>c.Id.Equals(id) && c.DeletedAt == null).FirstOrDefaultAsync();

        public Task<Permission> GetPermissionByNameAsync(string name) => FindByCondition(c => c.Name.Equals(name) && c.DeletedAt == null).FirstOrDefaultAsync();

<<<<<<< HEAD
        public async Task<IEnumerable<Permission>> GetPermissionsAsync() => await FindByCondition(c => c.DeletedAt == null).OrderByDescending(r => r.Id).ToListAsync();
=======
        public async Task<IEnumerable<Permission>> GetPermissionsAsync() => await FindByCondition(c => c.DeletedAt == null).ToListAsync();
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

        public Task UpdatePermissionAsync(Permission permission) => UpdateAsync(permission);
    }
}

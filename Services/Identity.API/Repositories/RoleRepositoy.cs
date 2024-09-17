using Identity.API.Persistence;
using Infrastructure.Repositories;
using Identity.API.Entites;
using Identity.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Contracts.Domains.Interfaces;

namespace Identity.API.Repositories
{
    public class RoleRepositoy : RepositoryBase<Role, int, IdentityDbContext>, IRoleRepository
    {
        public RoleRepositoy(IdentityDbContext dbContext, IUnitOfWork<IdentityDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreateRoleAsync(Role role) => CreateAsync(role);

        public async Task DeleteRoleAsync(int id)
        {
            var role = await GetRoleByIdAsync(id);
            if(role != null )
            {
                await DeleteAsync(role);
            }    
        }

        public Task<Role> GetRoleByIdAsync(int id) => FindByCondition(c=>c.Id.Equals(id)).FirstOrDefaultAsync();

        public Task<IEnumerable<RoleDetail>> GetRoleDetailsByRoleId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetRolesAsync() => await FindAll().ToListAsync();  

        public Task UpdateRoleAsync(Role role) => UpdateAsync(role);

        public Task<IEnumerable<RoleDetail>> UpdateRoleDetailsByRoleId(int id)
        {
            throw new NotImplementedException();
        }
    }
}

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
        private readonly IdentityDbContext _context;    
        public RoleRepositoy(IdentityDbContext dbContext, IUnitOfWork<IdentityDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            _context = dbContext;
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

        public async Task DeleteRoleDetailByRoleIdAsync(int roleId)
        {
            var roleDetailsToDelete = await _context.RoleDetails.Where(x => x.RoleId == roleId).ToListAsync();
            _context.RoleDetails.RemoveRange(roleDetailsToDelete);
        }

        public Task<Role> GetRoleByIdAsync(int id) => FindByCondition(c=>c.Id.Equals(id)).FirstOrDefaultAsync();

        public Task<IEnumerable<RoleDetail>> GetRoleDetailsByRoleId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetRolesAsync() => await FindAll().ToListAsync();  

        public Task UpdateRoleAsync(Role role) => UpdateAsync(role);

        public async Task<RoleDetail> UpdateRoleDetailByRoleIdAsync(int id,RoleDetail item)
        {
            var roleDetailEntity = await _context.RoleDetails.Where(c => c.RoleId == id && c.PermissionId == item.PermissionId).FirstOrDefaultAsync();
            if(roleDetailEntity == null)
            {
                var newRoleDetail = new RoleDetail()
                {
                    PermissionId = item.PermissionId,
                    RoleId = item.RoleId,
                    Status = item.Status,   
                    ActionName = item.ActionName,
                };
                await _context.RoleDetails.AddAsync(newRoleDetail);
                await _context.SaveChangesAsync();
                return newRoleDetail;
            }
            else
            {
                roleDetailEntity.ActionName = item.ActionName;  
                roleDetailEntity.Status = item.Status;

                _context.RoleDetails.Update(roleDetailEntity);
                await _context.SaveChangesAsync();  
                return roleDetailEntity;
            }
        }
		public async Task<List<RoleDetail>> GetRoleDetailsByRoleIdAsync(int roleId)
		{
			return await _context.RoleDetails
				.Where(rd => rd.RoleId == roleId)
				.ToListAsync();
		}
	}
}

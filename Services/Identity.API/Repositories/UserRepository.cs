using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;
using Identity.API.Repositories.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repositories
{
    public class UserRepository : RepositoryBase<User, int, IdentityDbContext>, IUserRepository
    {
        public UserRepository(IdentityDbContext dbContext, IUnitOfWork<IdentityDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task<int> CreateUserAsync(User user) => CreateAsync(user);

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
			if (user != null)
			{
				user.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(user);
			}
		}
        public async Task<User> GetUserByIdAsync(int id) => await FindByCondition(c=>c.Id.Equals(id) && c.DeletedAt == null, false, c => c.Account, c => c.Account.Role, c => c.Account.Role.RoleDetails).FirstOrDefaultAsync();

<<<<<<< HEAD
        public async Task<IEnumerable<User>> GetUsersAsync() => await FindByCondition(c => c.DeletedAt == null,false, c=>c.Account, c=>c.Account.Role, c => c.Account.Role.RoleDetails).OrderByDescending(r => r.CreatedAt).ToListAsync();
=======
        public async Task<IEnumerable<User>> GetUsersAsync() => await FindByCondition(c => c.DeletedAt == null,false, c=>c.Account, c=>c.Account.Role, c => c.Account.Role.RoleDetails).ToListAsync();
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

        public Task UpdateUserAsync(User user) => UpdateAsync(user);
    }
}

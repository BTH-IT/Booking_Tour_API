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
                 await DeleteAsync(user);   
            }
        }
        public async Task<User> GetUserByIdAsync(int id) => await FindByCondition(c=>c.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetUsersAsync() => await FindAll().ToListAsync();

        public Task UpdateUserAsync(User user) => UpdateAsync(user);
    }
}

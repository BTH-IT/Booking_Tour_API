using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;

namespace Identity.API.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User,int,IdentityDbContext>
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}

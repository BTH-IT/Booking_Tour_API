using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;
using System.Linq.Expressions;

namespace Identity.API.Repositories.Interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account,int,IdentityDbContext>
    {
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task<Account> GetAccountByEmailAsync(string email);
        Task CreateAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
    }
}

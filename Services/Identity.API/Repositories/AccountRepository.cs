using Contracts.Domains.Interfaces;
using Identity.API.Entites;
using Identity.API.Persistence;
using Identity.API.Repositories.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repositories
{
    public class AccountRepository : RepositoryBase<Account, int, IdentityDbContext>, IAccountRepository
    {
        public AccountRepository(IdentityDbContext dbContext, IUnitOfWork<IdentityDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreateAccountAsync(Account account) => CreateAsync(account);

        public async Task DeleteAccountAsync(int id)
        {
            var account = await GetAccountByIdAsync(id);
            if (account != null) 
            { 
                await DeleteAsync(account);
            }
        }

        public Task<Account> GetAccountByEmailAsync(string email) => FindByCondition(c => c.Email.Equals(email),false,c => c.Role, c => c.Role.RoleDetails).SingleOrDefaultAsync();
        public Task<Account> GetAccountByIdAsync(int id) => FindByCondition(c => c.Id.Equals(id),false,c=>c.Role,c=>c.Role.RoleDetails).SingleOrDefaultAsync();

        public async Task<IEnumerable<Account>> GetAccountsAsync() => await FindAll().ToListAsync();

        public Task UpdateAccountAsync(Account account) => UpdateAsync(account);
    }
}

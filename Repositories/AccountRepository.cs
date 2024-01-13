using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Models;
using BookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IRoleRepository _roleRepository;

        public AccountRepository(DataContext context, IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }

        public async Task<Account> GetByEmail(string email)
        {
            var result = await _context.Accounts.Where(acc => acc.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).SingleOrDefaultAsync();

            return result;
        }

        public async Task<Account> Update(AccountRequestDTO item)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(item.Id);

                if (account == null)
                {
                    return null;
                }

                account.Email = item.Email;
                account.Password = item.Password;

                await _context.SaveChangesAsync();

                return account;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(id);

                if (account == null) return false;

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Account>> GetAll()
        {
            List<Account> result = await _context.Accounts.ToListAsync();

            if (result == null) return new List<Account>();

            return result;
        }

        public async Task<Account> GetById(int id)
        {
            Account result = await _context.Accounts.FindAsync(id);

            if (result == null) return null;

            result.Role.RoleDetails = await _roleRepository.GetRoleDetailAllByRoleId(result.Role.Id);

            return result;
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(AccountRequestDTO item)
        {
            try
            {
                Account account = new Account
                {
                    Email = item.Email,
                    Password = item.Password,
                    Role = await _roleRepository.GetById(0),
                };

                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();

                return (true, account.Id);
            }
            catch (Exception ex)
            {
                return (false, -1);
            }
        }
    }
}

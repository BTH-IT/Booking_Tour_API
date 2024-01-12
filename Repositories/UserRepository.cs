using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Interfaces;
using BookingApi.Models;
using BookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepository;
        public UserRepository(DataContext context, IAccountRepository accountRepository) {
            _context = context;
            _accountRepository = accountRepository;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Include(u => u.Account)
                                        .ThenInclude(a => a.Role)
                                            .ThenInclude(r => r.RoleDetails)
                                        .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var result = await _context.Users.Include(u => u.Account)
                                                .ThenInclude(a => a.Role)
                                                    .ThenInclude(r => r.RoleDetails)
                                             .SingleOrDefaultAsync(u => u.Id == id);

            return result;
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(UserRequestDTO item)
        {
            try
            {
                User user = new User
                {
                    Fullname = item.Fullname,
                    BirthDate = item.BirthDate,
                    Country = item.Country,
                    Phone = item.Phone,
                    Gender = item.Gender,
                    Account = await _accountRepository.GetById(item.Id)
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return (true, user.Id);
            }
            catch (Exception ex)
            {
                return (false, -1);
            }
        }

        public async Task<User> Update(UserRequestDTO item)
        {
            try
            {
                var user = await _context.Users.FindAsync(item.Id);

                if (user == null)
                {
                    return null;
                }

                user.Fullname = item.Fullname;
                user.BirthDate = item.BirthDate;
                user.Phone = item.Phone;
                user.Country = item.Country;

                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

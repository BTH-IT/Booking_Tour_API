
using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();
        Task<Account> GetById(int id);
        Task<Account> GetByEmail(string email);
        Task<(bool isSuccess, int insertedItemId)> Insert(AccountRequestDTO item);
        Task<Account> Update(AccountRequestDTO item);
        Task<bool> Delete(int id);
    }
}

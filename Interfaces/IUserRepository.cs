using BookingApi.DTO;
using BookingApi.Models;
using Microsoft.VisualBasic;

namespace BookingApi.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(UserRequestDTO item);
        Task<User> Update(UserRequestDTO item);
        Task<bool> Delete(int id);
    }
}

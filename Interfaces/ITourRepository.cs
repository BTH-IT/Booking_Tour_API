using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Interfaces
{
    public interface ITourRepository
    {
        Task<List<Tour>> GetAll();
        Task<Tour> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(TourRequestDTO item);
        Task<Tour> Update(TourRequestDTO item);
        Task<bool> Delete(int id);
    }
}

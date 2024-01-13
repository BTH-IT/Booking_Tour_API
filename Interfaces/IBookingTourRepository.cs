using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Interfaces
{
    public interface IBookingTourRepository
    {
        Task<List<BookingTour>> GetAll();
        Task<BookingTour> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(BookingTourRequestDTO item);
        Task<BookingTour> Update(BookingTourRequestDTO item);
        Task<bool> Delete(int id);
    }
}

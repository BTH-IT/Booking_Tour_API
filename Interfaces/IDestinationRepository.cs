using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Interfaces
{
    public interface IDestinationRepository
    {
        Task<List<Destination>> GetAll();
        Task<Destination> GetById(int id);
        Task<(bool isSuccess, int insertedItemId)> Insert(DestinationRequestDTO item);
        Task<Destination> Update(DestinationRequestDTO item);
        Task<bool> Delete(int id);
    }
}

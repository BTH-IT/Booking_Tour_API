using Contracts.Domains.Interfaces;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface ITourRoomRepository : IRepositoryBase<TourRoom, int, TourDbContext>
    {
        Task<IEnumerable<TourRoom>> GetTourRoomsByTourIdAsync(int tourId);
        Task<int> CreateTourRoomAsync(TourRoom tourRoom);
        Task<int> UpdateTourRoomAsync(TourRoom tourRoom);
        Task DeleteTourRoomAsync(int id);
    }
}

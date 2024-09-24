using Contracts.Domains.Interfaces;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface ITourRepository : IRepositoryBase<TourEntity, int, TourDbContext>
    {
        Task<IEnumerable<TourEntity>> GetToursAsync();
        Task<TourEntity> GetTourByIdAsync(int id);
        Task<TourEntity> GetTourByNameAsync(string name);
        Task CreateTourAsync(TourEntity Tour);
        Task UpdateTourAsync(TourEntity Tour);
        Task DeleteTourAsync(int id);
    }
}

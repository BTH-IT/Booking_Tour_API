using Contracts.Domains.Interfaces;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface IDestinationRepository : IRepositoryBase<DestinationEntity, int, TourDbContext>
    {
        Task<IEnumerable<DestinationEntity>> GetDestinationsAsync();
        Task<DestinationEntity> GetDestinationByIdAsync(int id);
        Task<DestinationEntity> GetDestinationByNameAsync(string name);
<<<<<<< HEAD
        Task<int> CreateDestinationAsync(DestinationEntity destination);
		Task<int> UpdateDestinationAsync(DestinationEntity destination);
=======
        Task CreateDestinationAsync(DestinationEntity destination);
        Task UpdateDestinationAsync(DestinationEntity destination);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        Task DeleteDestinationAsync(int id);
    }
}

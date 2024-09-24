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
        Task CreateDestinationAsync(DestinationEntity destination);
        Task UpdateDestinationAsync(DestinationEntity destination);
        Task DeleteDestinationAsync(int id);
    }
}

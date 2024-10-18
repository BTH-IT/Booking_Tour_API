using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tour.API.Entities;
using Tour.API.Persistence;
using Tour.API.Repositories.Interfaces;

namespace Tour.API.Repositories
{
    public class DestinationRepository : RepositoryBase<DestinationEntity, int, TourDbContext>, IDestinationRepository
    {
        public DestinationRepository(TourDbContext dbContext, IUnitOfWork<TourDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

		public async Task<IEnumerable<DestinationEntity>> GetDestinationsAsync() =>
			 await FindAll(false).ToListAsync(); 
		
        public Task<DestinationEntity> GetDestinationByNameAsync(string name) =>
	         FindByCondition(d => d.Name.Equals(name), false).SingleOrDefaultAsync();

		public Task<DestinationEntity> GetDestinationByIdAsync(int id) =>
			 FindByCondition(d => d.Id == id, false).SingleOrDefaultAsync();

		public Task<int> CreateDestinationAsync(DestinationEntity destination) => CreateAsync(destination);

		public Task<int> UpdateDestinationAsync(DestinationEntity destination) => UpdateAsync(destination);

		public async Task DeleteDestinationAsync(int id)
		{
			var destination = await GetDestinationByIdAsync(id);
			if (destination != null)
			{
				await DeleteAsync(destination);
			}
		}
	}
}

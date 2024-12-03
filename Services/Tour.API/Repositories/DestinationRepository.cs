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

<<<<<<< HEAD
		public async Task<IEnumerable<DestinationEntity>> GetDestinationsAsync() =>
			 await FindAll(false)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

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
=======
        // Tạo mới một Destination
        public Task CreateDestinationAsync(DestinationEntity destination) => CreateAsync(destination);

        // Xóa một Destination dựa trên ID
        public async Task DeleteDestinationAsync(int id)
        {
            var destination = await GetDestinationByIdAsync(id);
            if (destination != null)
            {
                await DeleteAsync(destination);
            }
        }

        // Tìm Destination theo tên
        public Task<DestinationEntity> GetDestinationByNameAsync(string name)
        {
            return FindByCondition(d => d.Name.Equals(name), false)
                .SingleOrDefaultAsync();
        }

        // Tìm Destination theo ID
        public Task<DestinationEntity> GetDestinationByIdAsync(int id)
        {
            return FindByCondition(d => d.Id == id, false)
                .SingleOrDefaultAsync();
        }

        // Lấy tất cả các Destination
        public async Task<IEnumerable<DestinationEntity>> GetDestinationsAsync()
        {
            return await FindAll(false).ToListAsync();
        }

        // Cập nhật thông tin của Destination
        public Task UpdateDestinationAsync(DestinationEntity destination) => UpdateAsync(destination);
    }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}

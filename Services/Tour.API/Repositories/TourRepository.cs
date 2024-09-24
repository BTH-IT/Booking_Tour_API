using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tour.API.Entities;
using Tour.API.Persistence;
using Tour.API.Repositories.Interfaces;

namespace Tour.API.Repositories
{
    public class TourRepository : RepositoryBase<TourEntity, int, TourDbContext>, ITourRepository
    {
        public TourRepository(TourDbContext dbContext, IUnitOfWork<TourDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        // Tạo mới một Tour
        public Task CreateTourAsync(TourEntity tour) => CreateAsync(tour);

        // Xóa một Tour dựa trên ID
        public async Task DeleteTourAsync(int id)
        {
            var tour = await GetTourByIdAsync(id);
            if (tour != null)
            {
                await DeleteAsync(tour);
            }
        }

        // Tìm Tour theo ID
        public Task<TourEntity> GetTourByIdAsync(int id)
        {
            return FindByCondition(t => t.Id == id, false)
                .SingleOrDefaultAsync();
        }

        // Tìm Tour theo tên
        public Task<TourEntity> GetTourByNameAsync(string name)
        {
            return FindByCondition(t => t.Name.Equals(name), false)
                .SingleOrDefaultAsync();
        }

        // Lấy tất cả các Tour
        public async Task<IEnumerable<TourEntity>> GetToursAsync()
        {
            return await FindAll(false).ToListAsync();
        }

        // Cập nhật thông tin của Tour
        public Task UpdateTourAsync(TourEntity tour) => UpdateAsync(tour);
    }
}

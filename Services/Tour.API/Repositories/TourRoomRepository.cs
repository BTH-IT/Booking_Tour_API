using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tour.API.Entities;
using Tour.API.Persistence;
using Tour.API.Repositories.Interfaces;

namespace Tour.API.Repositories
{
    public class TourRoomRepository : RepositoryBase<TourRoom, int, TourDbContext>, ITourRoomRepository
    {
        public TourRoomRepository(TourDbContext dbContext, IUnitOfWork<TourDbContext> unitOfWork) : base(dbContext, unitOfWork) { }

        public async Task<IEnumerable<TourRoom>> GetTourRoomsByTourIdAsync(int tourId) =>
            await FindByCondition(tr => tr.TourId == tourId && tr.DeletedAt == null, false).ToListAsync();

        public Task<int> CreateTourRoomAsync(TourRoom tourRoom) => CreateAsync(tourRoom);

        public Task<int> UpdateTourRoomAsync(TourRoom tourRoom) => UpdateAsync(tourRoom);

        public async Task DeleteTourRoomAsync(int id)
        {
            var tourRoom = await GetByIdAsync(id);
            if (tourRoom != null)
            {
                tourRoom.DeletedAt = DateTime.UtcNow;
                await UpdateAsync(tourRoom);
            }
        }
    }
}

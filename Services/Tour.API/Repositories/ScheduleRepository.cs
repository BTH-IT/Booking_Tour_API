using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tour.API.Entities;
using Tour.API.Persistence;
using Tour.API.Repositories.Interfaces;

namespace Tour.API.Repositories
{
    public class ScheduleRepository : RepositoryBase<Schedule, int, TourDbContext>, IScheduleRepository
    {
        public ScheduleRepository(TourDbContext dbContext, IUnitOfWork<TourDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

		public async Task<IEnumerable<Schedule>> GetSchedulesAsync() =>
			await FindByCondition(r => true, false, r => r.Tour, r => r.Tour.Destination)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        public Task<Schedule> GetScheduleByNameAsync(string name) =>
			 FindByCondition(s => s.Tour.Name.Equals(name), false, r => r.Tour, r => r.Tour.Destination).SingleOrDefaultAsync();

		public Task<Schedule> GetScheduleByIdAsync(int id) =>
			FindByCondition(s => s.Id == id, false, r => r.Tour, r => r.Tour.Destination).SingleOrDefaultAsync();

		public async Task<IEnumerable<Schedule>> GetSchedulesByTourIdAsync(int tourId) =>
			 await FindByCondition(s => s.TourId == tourId, false, r => r.Tour, r => r.Tour.Destination)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
		
        public Task<int> CreateScheduleAsync(Schedule schedule) => CreateAsync(schedule);

		public Task<int> UpdateScheduleAsync(Schedule schedule) => UpdateAsync(schedule);
		
        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await GetScheduleByIdAsync(id);
            if (schedule != null)
            {
                await DeleteAsync(schedule);
            }
        }
    }
}

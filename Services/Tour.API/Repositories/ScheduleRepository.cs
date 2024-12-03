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

<<<<<<< HEAD
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
		
=======
        // Tạo mới một Schedule
        public Task CreateScheduleAsync(Schedule schedule) => CreateAsync(schedule);

        // Xóa một Schedule dựa trên ID
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await GetScheduleByIdAsync(id);
            if (schedule != null)
            {
                await DeleteAsync(schedule);
            }
        }
<<<<<<< HEAD
=======

        // Tìm Schedule theo ID
        public Task<Schedule> GetScheduleByIdAsync(int id)
        {
            return FindByCondition(s => s.Id == id, false)
                .SingleOrDefaultAsync();
        }

        // Tìm Schedule theo tên (nếu có trường tên trong Schedule)
        public Task<Schedule> GetScheduleByNameAsync(string name)
        {
            return FindByCondition(s => s.Tour.Name.Equals(name), false) // Giả định Tour có tên
                .SingleOrDefaultAsync();
        }

        // Lấy tất cả các Schedule
        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        {
            return await FindAll(false).ToListAsync();
        }

        // Cập nhật thông tin của Schedule
        public Task UpdateScheduleAsync(Schedule schedule) => UpdateAsync(schedule);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
}

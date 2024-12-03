using Contracts.Domains.Interfaces;
<<<<<<< HEAD
using System.Collections;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface IScheduleRepository : IRepositoryBase<Schedule, int, TourDbContext>
    {
        Task<IEnumerable<Schedule>> GetSchedulesAsync();
        Task<Schedule> GetScheduleByIdAsync(int id);
<<<<<<< HEAD
        Task<IEnumerable<Schedule>> GetSchedulesByTourIdAsync(int tourId);
		Task<Schedule> GetScheduleByNameAsync(string name);
		Task<int> CreateScheduleAsync(Schedule schedule);
		Task<int> UpdateScheduleAsync(Schedule schedule);
=======
        Task<Schedule> GetScheduleByNameAsync(string name); 
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        Task DeleteScheduleAsync(int id);
    }
}

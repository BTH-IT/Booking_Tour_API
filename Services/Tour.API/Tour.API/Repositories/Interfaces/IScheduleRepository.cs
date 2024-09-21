﻿using Contracts.Domains.Interfaces;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface IScheduleRepository : IRepositoryBase<Schedule, int, TourDbContext>
    {
        Task<IEnumerable<Schedule>> GetSchedulesAsync();
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task<Schedule> GetScheduleByNameAsync(string name); 
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}

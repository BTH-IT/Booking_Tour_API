﻿using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ApiResponse<List<ScheduleResponseDTO>>> GetAllAsync();
        Task<ApiResponse<ScheduleResponseDTO>> GetByIdAsync(int id);
        Task<ApiResponse<List<ScheduleResponseDTO>>> GetByTourIdAsync(int tourId);
		Task<ApiResponse<ScheduleResponseDTO>> CreateAsync(ScheduleRequestDTO item);
        Task<ApiResponse<ScheduleResponseDTO>> UpdateAsync(int id, ScheduleRequestDTO item);
        Task<ApiResponse<int>> DeleteAsync(int id);
    }
}

using Contracts.Domains.Interfaces;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
    public interface ITourRepository : IRepositoryBase<TourEntity, int, TourDbContext>
    {
        Task<IEnumerable<TourEntity>> GetToursAsync(); // Lấy các tour chưa bị xóa
        Task<TourEntity> GetTourByIdAsync(int id); // Lấy tour theo ID nếu chưa bị xóa
        Task<TourEntity> GetTourByNameAsync(string name); // Lấy tour theo tên nếu chưa bị xóa
        Task CreateTourAsync(TourEntity tour); // Tạo tour mới
        Task UpdateTourAsync(TourEntity tour); // Cập nhật tour
        Task SoftDeleteTourAsync(int id); // Xóa giả (đánh dấu DeletedAt)
    }
}

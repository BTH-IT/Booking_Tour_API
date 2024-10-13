using Contracts.Domains.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Entities;
using Tour.API.Persistence;

namespace Tour.API.Repositories.Interfaces
{
	public class TourSearchResult
	{
		public List<TourEntity> Tours { get; set; } = new List<TourEntity>();
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
	}

	public interface ITourRepository : IRepositoryBase<TourEntity, int, TourDbContext>
    {
        Task<IEnumerable<TourEntity>> GetToursAsync(); 
        Task<TourEntity> GetTourByIdAsync(int id); 
        Task<TourEntity> GetTourByNameAsync(string name);
        Task CreateTourAsync(TourEntity tour); 
        Task UpdateTourAsync(TourEntity tour); 
        Task SoftDeleteTourAsync(int id);
		Task<TourSearchResult> SearchToursAsync(TourSearchRequestDTO searchRequest);
	}
}

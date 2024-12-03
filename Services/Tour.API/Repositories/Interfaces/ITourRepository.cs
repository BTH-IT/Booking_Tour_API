using Contracts.Domains.Interfaces;
using Shared.DTOs;
<<<<<<< HEAD
=======
using Shared.Helper;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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
<<<<<<< HEAD
		Task<int> CreateTourAsync(TourEntity tour);
		Task<int> UpdateTourAsync(TourEntity tour); 
        Task DeleteTourAsync(int id);
=======
        Task CreateTourAsync(TourEntity tour); 
        Task UpdateTourAsync(TourEntity tour); 
        Task SoftDeleteTourAsync(int id);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		Task<TourSearchResult> SearchToursAsync(TourSearchRequestDTO searchRequest);
	}
}

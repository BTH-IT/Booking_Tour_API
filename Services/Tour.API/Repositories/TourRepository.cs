using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
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

        public async Task<IEnumerable<TourEntity>> GetToursAsync() => 
            await FindByCondition(r => r.DeletedAt == null, false, r => r.Destination) 
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public Task<TourEntity?> GetTourByIdAsync(int id) =>
			 FindByCondition(t => t.Id == id, false, r => r.Destination).SingleOrDefaultAsync();

		public Task<TourEntity?> GetTourByNameAsync(string name) =>
			 FindByCondition(t => t.Name.Equals(name), false, r => r.Destination).SingleOrDefaultAsync();

		public Task<int> CreateTourAsync(TourEntity tour) => CreateAsync(tour);

		public Task<int> UpdateTourAsync(TourEntity tour) => UpdateAsync(tour);

		public async Task DeleteTourAsync(int id)
		{
			var tour = await GetTourByIdAsync(id);
			if (tour != null)
			{
				tour.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(tour);
			}
		}

        public async Task<TourSearchResult> SearchToursAsync(TourSearchRequestDTO searchRequest)
        {
            var query = FindByCondition(t => t.DeletedAt == null, false, t => t.Destination);

            var minPrice = (await query.MinAsync(e => (decimal?)e.Price)) ?? 0m;
            var maxPrice = (await query.MaxAsync(e => (decimal?)e.Price)) ?? 0m;

            if (!string.IsNullOrWhiteSpace(searchRequest.Keyword))
            {
                query = query.Where(t => t.Name.Contains(searchRequest.Keyword) || t.Detail.Contains(searchRequest.Keyword));
            }

            if (searchRequest.Destinations?.Any() == true)
            {
                query = query.Where(t => searchRequest.Destinations.Contains(t.DestinationId.ToString()));
            }

            if (searchRequest.MinPrice.HasValue)
            {
                query = query.Where(t => t.Price >= searchRequest.MinPrice);
            }

            if (searchRequest.MaxPrice.HasValue)
            {
                query = query.Where(t => t.Price <= searchRequest.MaxPrice);
            }

            if (searchRequest.StartDate.HasValue)
            {
                query = query.Where(t => t.DateFrom >= searchRequest.StartDate);
            }

            if (searchRequest.EndDate.HasValue)
            {
                query = query.Where(t => t.DateTo <= searchRequest.EndDate);
            }

            if (searchRequest.Rating.HasValue)
            {
                query = query.Where(t => t.Rate <= searchRequest.Rating);
            }

            var tours = await query.ToListAsync();

            var currentDate = DateTime.Now;

            var sortedTours = searchRequest.SortBy?.ToLower() switch
            {
                "releasedate" => searchRequest.IsDescending ?
                    tours.OrderByDescending(t => (currentDate - t.DateFrom).TotalDays).ToList() :
                    tours.OrderBy(t => (currentDate - t.DateFrom).TotalDays).ToList(),
                "tourdate" => searchRequest.IsDescending ?
                    tours.OrderByDescending(t => (t.DateTo - t.DateFrom).TotalDays).ToList() :
                    tours.OrderBy(t => (t.DateTo - t.DateFrom).TotalDays).ToList(),
                "name" => searchRequest.IsDescending ?
                    tours.OrderByDescending(t => t.Name).ToList() :
                    tours.OrderBy(t => t.Name).ToList(),
                "price" => searchRequest.IsDescending ?
                    tours.OrderByDescending(t => t.Price).ToList() :
                    tours.OrderBy(t => t.Price).ToList(),
                "rating" => searchRequest.IsDescending ?
                    tours.OrderByDescending(t => t.Rate).ToList() :
                    tours.OrderBy(t => t.Rate).ToList(),
                _ => tours.OrderBy(t => t.Name).ToList()
            };

            var totalItems = sortedTours.Count;

            var paginatedTours = sortedTours
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToList();

            return new TourSearchResult
            {
                Tours = paginatedTours,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }
    }
}

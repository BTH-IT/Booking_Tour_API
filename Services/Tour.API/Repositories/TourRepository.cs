using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
<<<<<<< HEAD
=======
using Shared.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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

<<<<<<< HEAD
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

=======
		// Tạo mới một Tour
		public Task CreateTourAsync(TourEntity tour) => CreateAsync(tour);

		// Xóa một Tour dựa trên ID
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public async Task DeleteTourAsync(int id)
		{
			var tour = await GetTourByIdAsync(id);
			if (tour != null)
			{
<<<<<<< HEAD
=======
				await DeleteAsync(tour);
			}
		}

		// Tìm Tour theo ID
		public Task<TourEntity?> GetTourByIdAsync(int id)
		{
			return FindByCondition(t => t.Id == id, false)
				.SingleOrDefaultAsync();
		}

		// Tìm Tour theo tên
		public Task<TourEntity?> GetTourByNameAsync(string name)
		{
			return FindByCondition(t => t.Name.Equals(name), false)
				.SingleOrDefaultAsync();
		}

		// Lấy tất cả các Tour
		public async Task<IEnumerable<TourEntity>> GetToursAsync()
		{
			return await FindAll(false).ToListAsync();
		}

		// Cập nhật thông tin của Tour
		public Task UpdateTourAsync(TourEntity tour) => UpdateAsync(tour);

		public async Task SoftDeleteTourAsync(int id)
		{
			var tour = await GetTourByIdAsync(id);
			if (tour != null)
			{
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
				tour.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(tour);
			}
		}

<<<<<<< HEAD
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
=======
		public async Task<TourSearchResult> SearchToursAsync(TourSearchRequestDTO searchRequest)
		{
			// Lấy danh sách tất cả các tour
			var tourList = FindAll();
			var query = tourList.Where(t => t.DeletedAt == null);

			// Kiểm tra null và tính minPrice, maxPrice với toán tử null-coalescing (??)
			var minPrice = (await query.MinAsync(e => (decimal?)e.Price)) ?? 0m;
			var maxPrice = (await query.MaxAsync(e => (decimal?)e.Price)) ?? 0m;

			// Lọc theo các điều kiện tìm kiếm
			if (!string.IsNullOrWhiteSpace(searchRequest.Keyword))
			{
				query = query.Where(t => t.Name.Contains(searchRequest.Keyword) || t.Detail.Contains(searchRequest.Keyword));
			}
			if (searchRequest.MinPrice.HasValue && searchRequest.MaxPrice.HasValue)
			{
				query = query.Where(t => t.Price >= searchRequest.MinPrice.Value && t.Price <= searchRequest.MaxPrice.Value);
			}
			if (searchRequest.StartDate.HasValue && searchRequest.EndDate.HasValue)
			{
				query = query.Where(t => t.DateFrom >= searchRequest.StartDate && t.DateTo <= searchRequest.EndDate);
			}
			else if (searchRequest.StartDate.HasValue)
			{
				query = query.Where(t => t.DateFrom >= searchRequest.StartDate);
			}
			else if (searchRequest.EndDate.HasValue)
			{
				query = query.Where(t => t.DateTo <= searchRequest.EndDate);
			}
			if (searchRequest.Rating.HasValue)
			{
				query = query.Where(t => t.Rate <= searchRequest.Rating);
			}
			if (searchRequest.Activities?.Any() == true)
			{
				query = query.Where(t => t.ActivityList.Any(a => searchRequest.Activities.Contains(a)));
			}
			if (searchRequest.Destinations?.Any() == true)
			{
				query = query.Where(t => searchRequest.Destinations.Contains(t.DestinationId.ToString()));
			}

			// Đếm tổng số kết quả trước khi sắp xếp
			var totalItems = await query.CountAsync();

			// Lấy danh sách TourEntity từ database
			var queryResult = await query.ToListAsync();

			// Sắp xếp kết quả trong bộ nhớ
			if (!string.IsNullOrEmpty(searchRequest.SortBy))
			{
				switch (searchRequest.SortBy.ToLower())
				{
					case "releasedate":
						queryResult = searchRequest.IsDescending
							? queryResult.OrderByDescending(t => (DateTime.Now - t.DateFrom).TotalDays).ToList()
							: queryResult.OrderBy(t => (DateTime.Now - t.DateFrom).TotalDays).ToList();
						break;

					case "tourdate":
						queryResult = searchRequest.IsDescending
							? queryResult.OrderByDescending(t => (t.DateTo - t.DateFrom).TotalDays).ToList()
							: queryResult.OrderBy(t => (t.DateTo - t.DateFrom).TotalDays).ToList();
						break;

					case "name":
						queryResult = searchRequest.IsDescending
							? queryResult.OrderByDescending(t => t.Name).ToList()
							: queryResult.OrderBy(t => t.Name).ToList();
						break;

					case "price":
						queryResult = searchRequest.IsDescending
							? queryResult.OrderByDescending(t => t.Price).ToList()
							: queryResult.OrderBy(t => t.Price).ToList();
						break;

					case "rating":
						queryResult = searchRequest.IsDescending
							? queryResult.OrderByDescending(t => t.Rate).ToList()
							: queryResult.OrderBy(t => t.Rate).ToList();
						break;

					default:
						queryResult = queryResult.OrderBy(t => t.Name).ToList();
						break;
				}
			}

			// Thực hiện phân trang
			var paginatedTours = queryResult
				.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
				.Take(searchRequest.PageSize)
				.ToList();

			// Trả về kết quả tìm kiếm với danh sách tour, giá trị min/max của price
			return new TourSearchResult
			{
				Tours = paginatedTours,
				MinPrice = minPrice,
				MaxPrice = maxPrice
			};
		}
	}
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}

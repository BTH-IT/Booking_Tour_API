using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		// Tạo mới một Tour
		public Task CreateTourAsync(TourEntity tour) => CreateAsync(tour);

		// Xóa một Tour dựa trên ID
		public async Task DeleteTourAsync(int id)
		{
			var tour = await GetTourByIdAsync(id);
			if (tour != null)
			{
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
				tour.DeletedAt = DateTime.UtcNow;
				await UpdateAsync(tour);
			}
		}

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
}

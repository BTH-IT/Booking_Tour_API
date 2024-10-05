using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
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
        public Task<TourEntity> GetTourByIdAsync(int id)
        {
            return FindByCondition(t => t.Id == id, false)
                .SingleOrDefaultAsync();
        }

        // Tìm Tour theo tên
        public Task<TourEntity> GetTourByNameAsync(string name)
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

        public async Task<IEnumerable<TourEntity>> SearchToursAsync(TourSearchRequestDTO searchRequest)
        {
            var query = FindAll().Where(t => t.DeletedAt == null);

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

            var result = await query.ToListAsync();

            if (searchRequest.Activities?.Any() == true)
            {
                result = result.Where(t => t.ActivityList.Any(a => searchRequest.Activities.Contains(a))).ToList();
            }
            if (searchRequest.Destinations?.Any() == true)
            {
                result = result.Where(t => searchRequest.Destinations.Contains(t.DestinationId.ToString())).ToList();
            }
            if (!string.IsNullOrEmpty(searchRequest.SortBy))
            {
                switch (searchRequest.SortBy.ToLower())
                {
                    case "releasedate":
                        result = searchRequest.IsDescending
                            ? result.OrderByDescending(t => (DateTime.Now - t.DateFrom).TotalDays).ToList()
                            : result.OrderBy(t => (DateTime.Now - t.DateFrom).TotalDays).ToList();
                        break;

                    case "tourdate":
                        result = searchRequest.IsDescending
                            ? result.OrderByDescending(t => (t.DateTo - t.DateFrom).TotalDays).ToList()
                            : result.OrderBy(t => (t.DateTo - t.DateFrom).TotalDays).ToList();
                        break;

                    case "name":
                        result = searchRequest.IsDescending
                            ? result.OrderByDescending(t => t.Name).ToList()
                            : result.OrderBy(t => t.Name).ToList();
                        break;

                    case "price":
                        result = searchRequest.IsDescending
                            ? result.OrderByDescending(t => t.Price).ToList()
                            : result.OrderBy(t => t.Price).ToList();
                        break;

                    case "rating":
                        result = searchRequest.IsDescending
                            ? result.OrderByDescending(t => t.Rate).ToList()
                            : result.OrderBy(t => t.Rate).ToList();
                        break;

                    default:
                        result = result.OrderBy(t => t.Name).ToList();
                        break;
                }
            }
            return result;
        }
        public async Task<object> SearchToursWithPaginationAsync(TourSearchRequestDTO searchRequest)
        {
            var query = FindAll().Where(t => t.DeletedAt == null);

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
                query = query.Where(t => t.Rate >= searchRequest.Rating);
            }

            if (searchRequest.Activities?.Any() == true)
            {
                query = query.Where(t => t.ActivityList.Any(a => searchRequest.Activities.Contains(a)));
            }

            if (searchRequest.Destinations?.Any() == true)
            {
                query = query.Where(t => searchRequest.Destinations.Contains(t.DestinationId.ToString()));
            }

            if (!string.IsNullOrEmpty(searchRequest.SortBy))
            {
                switch (searchRequest.SortBy.ToLower())
                {
                    case "releasedate":
                        query = searchRequest.IsDescending
                            ? query.OrderByDescending(t => (DateTime.Now - t.DateFrom).TotalDays)
                            : query.OrderBy(t => (DateTime.Now - t.DateFrom).TotalDays);
                        break;

                    case "tourdate":
                        query = searchRequest.IsDescending
                            ? query.OrderByDescending(t => (t.DateTo - t.DateFrom).TotalDays)
                            : query.OrderBy(t => (t.DateTo - t.DateFrom).TotalDays);
                        break;

                    case "name":
                        query = searchRequest.IsDescending
                            ? query.OrderByDescending(t => t.Name)
                            : query.OrderBy(t => t.Name);
                        break;

                    case "price":
                        query = searchRequest.IsDescending
                            ? query.OrderByDescending(t => t.Price)
                            : query.OrderBy(t => t.Price);
                        break;

                    case "rating":
                        query = searchRequest.IsDescending
                            ? query.OrderByDescending(t => t.Rate)
                            : query.OrderBy(t => t.Rate);
                        break;

                    default:
                        query = query.OrderBy(t => t.Name);
                        break;
                }
            }

            var totalRecords = await query.CountAsync();

            var tours = await query
                .Skip((searchRequest.PageNumber.Value - 1) * searchRequest.PageSize.Value)
                .Take(searchRequest.PageSize.Value)
                .ToListAsync();

            return new
            {
                TotalRecords = totalRecords,
                PageNumber = searchRequest.PageNumber,
                PageSize = searchRequest.PageSize,
                Tours = tours
            };
        }
    }
}

using Microsoft.EntityFrameworkCore;
using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Interfaces;
using BookingApi.Models;

namespace BookingApi.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly DataContext _context;
        private readonly IDestinationRepository _destinationRepository;

        public TourRepository(DataContext context, IDestinationRepository destinationRepository)
        {
            _context = context;
            _destinationRepository = destinationRepository;
        }

        // Delete a Tour and its associated Schedules
        public async Task<bool> Delete(int id)
        {
            try
            {
                var tour = await _context.Tours.FindAsync(id);

                if (tour == null)
                    return false; // Tour not found, nothing to delete

                // Find all Schedules associated with the Tour
                var scheduleList = await _context.Schedules
                    .Where(s => s.Tour.Id == id)
                    .ToListAsync();

                // Remove each Schedule
                _context.Schedules.RemoveRange(scheduleList);

                // Remove the Tour
                _context.Tours.Remove(tour);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false;
            }
        }

        // Get all Tours
        public async Task<List<Tour>> GetAll()
        {
            return await _context.Tours.Include(t => t.Destination).ToListAsync();
        }

        // Get a Tour by its ID
        public async Task<Tour> GetById(int id)
        {
            return await _context.Tours.Include(t => t.Destination).SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Schedule>> GetSchedulesByTourId(int id)
        {
            return await _context.Schedules.Include(t => t.Tour).Where(t => t.Tour.Id == id).ToListAsync();
        }

        // Insert a new Tour
        public async Task<(bool isSuccess, int insertedItemId)> Insert(TourRequestDTO item)
        {
            try
            {
                // Create a new Tour entity and map properties from TourRequestDTO
                var tour = new Tour
                {
                    Name = item.Name,
                    MaxGuests = item.MaxGuests,
                    IsWifi = item.IsWifi,
                    Detail = item.Detail,
                    Expect = item.Expect,
                    Price = item.Price,
                    DateFrom = item.DateFrom,
                    DateTo = item.DateTo,
                    Rate = item.Rate,
                    Video = item.Video,
                    SalePercent = item.SalePercent,
                    PriceExcludeList = item.PriceExcludeList,
                    PriceIncludeList = item.PriceIncludeList,
                    ActivityList = item.ActivityList,
                    ImageList = item.ImageList,
                    DayList = item.DayList.ToArray(),
                    Destination = await _destinationRepository.GetById(item.DestinationId),
                    ReviewList = item.ReviewList.ToArray(),
                };

                // Add the new Tour to the database
                await _context.Tours.AddAsync(tour);

                DateTime dateFrom = item.DateFrom;
                DateTime dateTo = item.DateTo;

                while (dateFrom <= dateTo)
                {
                    DateTime dateStart = dateFrom;

                    dateFrom = dateFrom.AddDays(tour.Days.Length);

                    DateTime dateEnd = dateFrom;

                    Schedule schedule = new Schedule
                    {
                        DateStart = dateStart,
                        DateEnd = dateEnd,
                        Tour = tour,
                        AvailableSeats = tour.MaxGuests
                    };

                    // Assuming schedulesService is an instance of a service that creates new schedules
                    await _context.Schedules.AddAsync(schedule);

                    dateFrom = dateFrom.AddDays(1);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Add Schedule

                // Return success and the ID of the inserted Tour
                return (true, tour.Id);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return (false, -1);
            }
        }

        // Update an existing Tour
        public async Task<Tour> Update(TourRequestDTO item)
        {
            try
            {
                // Find the existing Tour by its ID
                var existingTour = await _context.Tours.FindAsync(item.Id);

                if (existingTour == null)
                {
                    // Handle the case where the Tour is not found
                    return null;
                }

                // Update properties of the existing Tour entity
                existingTour.Name = item.Name;
                existingTour.MaxGuests = item.MaxGuests;
                existingTour.IsWifi = item.IsWifi;
                existingTour.Detail = item.Detail;
                existingTour.Expect = item.Expect;
                existingTour.Price = item.Price;
                existingTour.DateFrom = item.DateFrom;
                existingTour.DateTo = item.DateTo;
                existingTour.Rate = item.Rate;
                existingTour.Video = item.Video;
                existingTour.SalePercent = item.SalePercent;
                existingTour.PriceExcludeList = item.PriceExcludeList;
                existingTour.PriceIncludeList = item.PriceIncludeList;
                existingTour.ActivityList = item.ActivityList;
                existingTour.ImageList = item.ImageList;
                existingTour.DayList = item.DayList.ToArray();
                existingTour.Destination = await _destinationRepository.GetById(item.DestinationId);
                existingTour.ReviewList = item.ReviewList.ToArray();

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the updated Tour entity
                return existingTour;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }
    }
}

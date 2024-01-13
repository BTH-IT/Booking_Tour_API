using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Interfaces;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingApi.Repositories
{
    public class BookingTourRepository : IBookingTourRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public BookingTourRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var bookingTour = await _context.BookingTours.FindAsync(id);

                if (bookingTour == null) return false;

                List<Schedule> scheduleList = await _context.Schedules.Where(s => s.Tour.Id == id).ToListAsync();

                foreach (Schedule schedule in scheduleList)
                {
                    _context.Schedules.Remove(schedule);
                    await _context.SaveChangesAsync();
                }

                _context.BookingTours.Remove(bookingTour);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<BookingTour>> GetAll()
        {
            return await _context.BookingTours.Include(bt => bt.User)
                .Include(bt => bt.Schedule)
                .ToListAsync();
        }

        public async Task<BookingTour> GetById(int id)
        {
            return await _context.BookingTours.Where(b => b.Id == id)
                        .Include(bt => bt.User)
                        .Include(bt => bt.Schedule)
                        .FirstOrDefaultAsync();
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(BookingTourRequestDTO item)
        {
            try
            {
                BookingTour bookingTour = new BookingTour
                {
                    User = await _userRepository.GetById(item.UserId),
                    Schedule = await _context.Schedules.FindAsync(item.ScheduleId),
                    Seats =item.Seats,
                    Umbrella =item.Umbrella,
                    IsCleaningFee =item.IsCleaningFee,
                    IsTip =item.IsTip,
                    IsEntranceTicket =item.IsEntranceTicket,
                    PriceTotal =item.PriceTotal,
                    Coupon =item.Coupon,
                    PaymentMethod =item.PaymentMethod,
                    TravellerList = item.Travellers,
                };

                await _context.BookingTours.AddAsync(bookingTour);
                await _context.SaveChangesAsync();

                return (true, bookingTour.Id);
            }
            catch (Exception ex)
            {
                return (false, -1);
            }
        }

        public async Task<BookingTour> Update(BookingTourRequestDTO item)
        {
            try
            {
                // Retrieve the existing BookingTour entity from the database
                var existingBookingTour = await _context.BookingTours
                    .Include(bt => bt.User)
                    .Include(bt => bt.Schedule)
                    .FirstOrDefaultAsync(bt => bt.Id == item.Id);

                if (existingBookingTour == null)
                {
                    // Handle the case where the BookingTour is not found
                    return null;
                }

                // Update the properties of the existing BookingTour entity
                existingBookingTour.User = await _userRepository.GetById(item.UserId);
                existingBookingTour.Schedule = await _context.Schedules.FindAsync(item.ScheduleId);
                existingBookingTour.Seats = item.Seats;
                existingBookingTour.Umbrella = item.Umbrella;
                existingBookingTour.IsCleaningFee = item.IsCleaningFee;
                existingBookingTour.IsTip = item.IsTip;
                existingBookingTour.IsEntranceTicket = item.IsEntranceTicket;
                existingBookingTour.PriceTotal = item.PriceTotal;
                existingBookingTour.Coupon = item.Coupon;
                existingBookingTour.PaymentMethod = item.PaymentMethod;
                existingBookingTour.TravellerList = item.Travellers;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return existingBookingTour;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

    }
}

using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Interfaces;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApi.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly DataContext _context;

        public DestinationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var destination = await _context.Destinations.FindAsync(id);

                if (destination == null)
                    return false;

                _context.Destinations.Remove(destination);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return false;
            }
        }

        public async Task<List<Destination>> GetAll()
        {
            return await _context.Destinations.ToListAsync();
        }

        public async Task<Destination> GetById(int id)
        {
            return await _context.Destinations.FindAsync(id);
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(DestinationRequestDTO item)
        {
            try
            {
                var destination = new Destination
                {
                    Name = item.Name,
                    Description = item.Description,
                    Url = item.Url
                };

                _context.Destinations.Add(destination);
                await _context.SaveChangesAsync();

                return (true, destination.Id);
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return (false, -1);
            }
        }

        public async Task<Destination> Update(DestinationRequestDTO item)
        {
            try
            {
                var destination = await _context.Destinations.FindAsync(item.Id);

                if (destination != null)
                {
                    destination.Name = item.Name;
                    destination.Description = item.Description;
                    destination.Url = item.Url;

                    await _context.SaveChangesAsync();
                }

                return destination;
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return null;
            }
        }
    }
}

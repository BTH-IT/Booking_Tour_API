using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Models;
using BookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _context;

        public PermissionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);

                if (permission == null)
                    return false;

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return false;
            }
        }

        public async Task<List<Permission>> GetAll()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission> GetById(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(PermissionRequestDTO item)
        {
            try
            {
                var permission = new Permission
                {
                    Name = item.Name,
                };

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();

                return (true, permission.Id);
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return (false, -1);
            }
        }

        public async Task<Permission> Update(PermissionRequestDTO item)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(item.Id);

                if (permission != null)
                {
                    permission.Name = item.Name;

                    await _context.SaveChangesAsync();
                }

                return permission;
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return null;
            }
        }
    }
}

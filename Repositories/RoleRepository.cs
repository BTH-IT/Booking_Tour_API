using System;
using BookingApi.Data;
using BookingApi.DTO;
using BookingApi.Models;
using BookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string[] _actionList = new[] { "CREATE", "READ", "UPDATE", "DELETE" };
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role == null) return false;

                _context.Roles.Remove(role);
                List<RoleDetail> roleDetailList = _context.RoleDetails.Where(roleDetail => roleDetail.RoleId == role.Id).ToList();

                foreach (RoleDetail roleDetail in roleDetailList)
                {
                    _context.RoleDetails.Remove(roleDetail);
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Role>> GetAll()
        {
            List<Role> result = await _context.Roles.ToListAsync();

            if (result == null) return new List<Role>();

            return result;
        }

        public async Task<Role> GetById(int id)
        {
            var result = await _context.Roles.FindAsync(id);

            return result;
        }

        public async Task<List<RoleDetail>> GetRoleDetailAllByRoleId(int id)
        {
            return await _context.RoleDetails.Where(rd => rd.RoleId == id).Select(rd => rd).ToListAsync();
        }

        public async Task<(bool isSuccess, int insertedItemId)> Insert(RoleRequestDTO item)
        {
            try
            {
                Role role = new Role
                {
                    RoleName = item.RoleName,
                    Status = true,
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                foreach (string action in _actionList)
                {
                    RoleDetail roleDetail = new RoleDetail
                    {
                        RoleId = role.Id,
                        ActionName = action,
                        Status = false
                    };

                    await _context.RoleDetails.AddAsync(roleDetail);
                }

                await _context.SaveChangesAsync();

                return (true, role.Id);
            }
            catch (Exception ex)
            {
                return (false, -1);
            }
        }

        public async Task<Role> Update(RoleRequestDTO item)
        {
            try
            {
                var role = await _context.Roles.FindAsync(item.Id);

                if (role == null)
                {
                    return null;
                }

                role.RoleName = item.RoleName;
                role.Status = item.Status;

                await _context.SaveChangesAsync();

                return role;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RoleDetail> UpdateRoleDetailByRoleId(RoleDetailDTO item)
        {
            try
            {
                var roleDetail = await _context.RoleDetails.Where(roleDetail => 
                    roleDetail.RoleId == item.RoleId && 
                    roleDetail.ActionName.ToUpperInvariant() == item.ActionName.ToUpperInvariant())
                        .SingleOrDefaultAsync();

                if (roleDetail == null)
                {
                    return null;
                }

                roleDetail.Status = item.Status;

                await _context.SaveChangesAsync();

                return roleDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

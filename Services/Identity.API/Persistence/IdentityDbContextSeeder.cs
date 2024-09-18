using Identity.API.Entites;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
namespace Identity.API.Persistence
{
    public class IdentityDbContextSeeder
    {
        private readonly IdentityDbContext _context;    
        private readonly ILogger _logger;

        public IdentityDbContextSeeder(IdentityDbContext context,ILogger logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public async Task InitialiseAsync()
        {
            try
            {
                _logger.Information("Initializing Db");
                if (_context.Database.IsMySql())
                {
                    await _context.Database.MigrateAsync();
                    _logger.Information("Initialed Db");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to Initialise database :{ex.Message}");
            }
        }
        public async Task IdentityDbSeedAsync()
        {
            _logger.Information("Begin : IdentityDbSeedAsync");
            if (! await _context.Permissions.AnyAsync())
            {
                await _context.AddRangeAsync(GetPermissions());
                await _context.SaveChangesAsync();

            }
            if (! await _context.Roles.AnyAsync())
            {
                await _context.AddRangeAsync(GetRoles()); 
                await _context.SaveChangesAsync();
            }
            if (! await _context.Accounts.AnyAsync())
            {
                var adminRole = await _context.Roles.Where(c=>c.RoleName.Equals("Admin")).FirstOrDefaultAsync();
                await _context.Accounts.AddAsync(new Account()
                {
                    Email = "admin@gmail.com",
                    Password = "admin@1234",
                    RoleId = adminRole.Id
                });
                await _context.SaveChangesAsync();

            }
            if (!await _context.Users.AnyAsync())
            {
                var account = await _context.Accounts.Where(c => c.Email.Equals("admin@gmail.com")).FirstOrDefaultAsync();
                await _context.Users.AddAsync(new User()
                {
                    Fullname= "Ad Van Min",
                    BirthDate = new DateTime(2003,05,25),
                    Country = "Chưa cập nhật",
                    Phone = "Chưa cập nhật",
                    Gender = "Khác",
                    AccountId = account.Id
                });
                await _context.SaveChangesAsync();
            }
            _logger.Information("End : IdentityDbSeedAsync");

        }
        private List<Role> GetRoles()
        {
            //ADMIN
            return new List<Role>()
            {
                new Role()
                {
                    RoleName = "Admin",
                    Status = true,
                },
                new Role()
                {
                    RoleName = "User",
                    Status = true
                },
            };
            // USER
        }
        public List<Permission> GetPermissions() 
        {
            return new List<Permission> {
                new Permission() { 
                    Name = "All"
                },
            };
        }
    }
}

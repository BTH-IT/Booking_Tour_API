using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace Booking.API.Persistence
{
    public class BookingDbContextSeeder
    {
        private readonly BookingDbContext _context;
        private readonly ILogger _logger;
        public BookingDbContextSeeder(BookingDbContext context, ILogger logger)
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
                    _logger.Information("Initialized Db");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to Initialise database :{ex.Message}");
            }
        }
    }
}

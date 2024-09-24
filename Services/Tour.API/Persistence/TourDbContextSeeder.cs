using Microsoft.EntityFrameworkCore;
using Tour.API.Persistence;
using ILogger = Serilog.ILogger;
namespace Room.API.Persistence
{
    public class TourDbContextSeeder
    {
        private readonly TourDbContext _context;
        private readonly ILogger _logger;

        public TourDbContextSeeder(TourDbContext context, ILogger logger)
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
    }
}

using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
namespace Room.API.Persistence
{
    public class RoomDbContextSeeder
    {
        private readonly RoomDbContext _context;
        private readonly ILogger _logger;

        public RoomDbContextSeeder(RoomDbContext context, ILogger logger)
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

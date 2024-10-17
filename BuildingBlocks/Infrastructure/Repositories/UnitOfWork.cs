using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        public UnitOfWork(TContext context)
        {
            _context = context;
        }
        public async Task<int> CommitAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

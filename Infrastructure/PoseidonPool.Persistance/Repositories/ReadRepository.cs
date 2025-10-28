using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Repositories;
using PoseidonPool.Domain.Entities;
using PoseidonPool.Persistance.Contexts;
using System.Linq.Expressions;

namespace PoseidonPool.Persistance.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly PoseidonPoolDBContext _context;

        public ReadRepository(PoseidonPoolDBContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id)); (Marker)
            //=> await Table.FindAsync(Guid.Parse(id));
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }

        public async Task<T> GetSingleAsync(System.Linq.Expressions.Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(System.Linq.Expressions.Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
    }
}

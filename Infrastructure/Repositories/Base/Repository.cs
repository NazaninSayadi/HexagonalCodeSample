using Domain.Entities;
using Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly SmartChargingContext _context;
        public Repository(SmartChargingContext context)
        {
            _context = context;
        }
        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> Get(params Expression<Func<T, object>>[] includes)
        {
            var set = _context.Set<T>().AsQueryable();

            if (includes != null)
                foreach (var include in includes)
                    set = set.Include(include);

            return await set.ToListAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }
    }
}

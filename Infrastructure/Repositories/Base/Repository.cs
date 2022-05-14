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
        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }
    }
}

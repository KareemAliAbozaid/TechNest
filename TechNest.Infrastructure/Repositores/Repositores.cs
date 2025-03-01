

using Microsoft.EntityFrameworkCore;
using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class Repositores<T> : IRepositores<T> where T : class
    {
        private readonly ApplicationDbcontext _context;
        private readonly DbSet<T> _dbSet;

        public Repositores(ApplicationDbcontext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public IQueryable<T> AsQueryable() => _dbSet.AsNoTracking();

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;  

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
          return  await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var entity =await _dbSet.FindAsync(id);
            if (entity != null)
            {
                return entity;
            }
            return null;
        }

        public Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }
    }
}

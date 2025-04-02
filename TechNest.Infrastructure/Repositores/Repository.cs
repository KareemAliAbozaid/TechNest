

using Microsoft.EntityFrameworkCore;
using TechNest.Application.Interfaces;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class Repository<T> : IRepositores<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly ApplicationDbcontext _context;

        public Repository(ApplicationDbcontext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
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
                await _context.SaveChangesAsync();
                return true;
            }
            return false;  

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(c => EF.Property<bool>(c, "IsDeleted") == false)
                .ToListAsync();
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

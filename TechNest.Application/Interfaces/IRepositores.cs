﻿

namespace TechNest.Application.Interfaces
{
    public interface IRepositores<T> where T : class
    {
        IQueryable<T> AsQueryable();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(Guid id);

        Task<bool> AddAsync(T entity);
      
        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
    }
}

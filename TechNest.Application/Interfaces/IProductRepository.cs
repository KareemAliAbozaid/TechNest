
using TechNest.Application.DTOs.Product;
using TechNest.Domain.Entites;

namespace TechNest.Application.Interfaces
{
    public interface IProductRepository : IRepositores<Product>
    {
        Task<bool> AddAsync(ProductCreateDto productCreateDto);
    }
}

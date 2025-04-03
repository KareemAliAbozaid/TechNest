
using TechNest.Domain.Entites;
using TechNest.Application.DTOs.Product;

namespace TechNest.Application.Interfaces
{
    public interface IProductRepository : IRepositores<Product>
    {
        Task<bool> AddAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
    }
}

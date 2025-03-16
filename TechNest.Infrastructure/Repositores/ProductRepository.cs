
using TechNest.Domain.Entites;
using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
        }
    }
}

using TechNest.Domain.Entites;
using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class Categoryrepository : Repository<Category>, ICategoryRepository
    {
        public Categoryrepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
        }
    }
}

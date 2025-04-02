using TechNest.Domain.Entites;
using TechNest.Application.Interfaces;
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

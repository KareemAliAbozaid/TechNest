using TechNest.Domain.Entites;
using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbcontext _context) : base(_context)
        {
        }
    }
}

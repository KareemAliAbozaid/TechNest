
using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbcontext _dbContext;
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(ApplicationDbcontext dbContext)
        {
            _dbContext = dbContext;
            CategoryRepository = new Categoryrepository(dbContext);
            ProductRepository = new ProductRepository(dbContext);
            PhotoRepository = new PhotoRepository(dbContext);
        }

    }
}

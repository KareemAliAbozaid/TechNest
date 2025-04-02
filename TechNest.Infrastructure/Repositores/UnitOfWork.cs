
using TechNest.Application.Interfaces;
using TechNest.Application.Services;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbcontext _dbContext;
        private readonly IImageManagmentService imageManagmentService;
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(ApplicationDbcontext dbContext, IImageManagmentService imageManagmentService)
        {
            _dbContext = dbContext;
            this.imageManagmentService = imageManagmentService;
            CategoryRepository = new Categoryrepository(dbContext);
            ProductRepository = new ProductRepository(dbContext, imageManagmentService);
            PhotoRepository = new PhotoRepository(dbContext);
           
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


    }
}

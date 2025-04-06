
using AutoMapper;
using TechNest.Application.Interfaces;
using TechNest.Application.Services;
using TechNest.Infrastructure.Data;

namespace TechNest.Infrastructure.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbcontext _dbContext;
        private readonly IImageManagmentService imageManagmentService;
       private readonly IMapper mapper;
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(ApplicationDbcontext dbContext, IImageManagmentService imageManagmentService, IMapper mapper)
        {
            _dbContext = dbContext;
            this.imageManagmentService = imageManagmentService;
            CategoryRepository = new Categoryrepository(dbContext);
            ProductRepository = new ProductRepository(dbContext, imageManagmentService, mapper);
            PhotoRepository = new PhotoRepository(dbContext);
            this.mapper = mapper;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


    }
}

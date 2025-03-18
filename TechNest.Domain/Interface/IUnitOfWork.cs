

namespace TechNest.Domain.Interface
{
   public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IPhotoRepository PhotoRepository { get; }

        Task<int> SaveChangesAsync();

    }
}

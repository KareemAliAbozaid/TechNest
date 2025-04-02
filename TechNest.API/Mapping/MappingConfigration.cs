using Mapster;
using TechNest.Application.DTOs.Product;
using TechNest.Domain.Entites;

namespace TechNest.API.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Product, ProductCreateDto>()
            //    .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null);
            //config.NewConfig<ProductCreateDto, Product>()
            //  .Ignore(dest => dest.Photos);
        }
    }
}

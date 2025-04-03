using Mapster;
using TechNest.Application.DTOs.Product;
using TechNest.Domain.Entites;

namespace TechNest.API.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
         
            config.NewConfig<UpdateProductDto, Product>()
              .Ignore(dest => dest.Photos);
        }
    }
}

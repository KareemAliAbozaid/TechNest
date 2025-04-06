using AutoMapper;
using TechNest.Domain.Entites;
using TechNest.Application.DTOs.Product;
using TechNest.Application.DTOs.Photos;

namespace Ecom.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            
            CreateMap<Photo, CreatePhotoDto>().ReverseMap();

            CreateMap<ProductCreateDto, Product>().ForMember(m => m.Photos, op => op.Ignore()).ReverseMap();

            CreateMap<UpdateProductDto, Product>().ForMember(m => m.Photos, op => op.Ignore()).ReverseMap();

            CreateMap<Product, GetProductDto>().ReverseMap();

        }
    }
}

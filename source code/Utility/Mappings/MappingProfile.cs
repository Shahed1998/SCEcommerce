using AutoMapper;
using Models.BusinessEntities;
using Models.Entities;

namespace Utility.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        }
    }


    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
        }
    }

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}

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
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryVM, Category>();
        }
    }

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductVM>();
            CreateMap<ProductVM, Product>();
        }
    }
}

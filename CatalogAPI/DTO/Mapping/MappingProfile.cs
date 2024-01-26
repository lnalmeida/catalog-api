using AutoMapper;
using CatalogAPI.Domain;

namespace CatalogAPI.DTO.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, InsertProductDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, InserCategoryDto>().ReverseMap();
    }
}
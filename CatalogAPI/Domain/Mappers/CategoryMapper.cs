using CatalogAPI.Domain.DTO;

namespace CatalogAPI.Domain.Mappers;

public static class CategoryMapper
{
    public static CategoryDTO MapToCategoryDTO(Category category)
    {
        if (category == null)
        {
            return null;
        }

        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryImageUrl = category.CategoryImageUrl,
        };
    }

    public static Category MapToCategory(CategoryDTO CategoryDTO)
    {
        if (CategoryDTO == null)
        {
            return null;
        }

        return new Category
        {
            CategoryId = CategoryDTO.CategoryId,
            CategoryName = CategoryDTO.CategoryName,
            CategoryImageUrl = CategoryDTO.CategoryImageUrl,
        };
    }
}


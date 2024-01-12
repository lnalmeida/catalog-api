using CatalogAPI.Domain.DTO;

namespace CatalogAPI.Domain.Mappers;

public static class ProductMapper
{
    public static ProductDTO MapToProductDTO(Product product)
    {
        if (product == null)
        {
            return null;
        }

        return new ProductDTO
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Created = (DateTime)product.Created,
        };
    }

    public static Product MapToProduct(ProductDTO productDTO)
    {
        if (productDTO == null)
        {
            return null;
        }

        return new Product
        {
            ProductId = productDTO.ProductId,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Stock = productDTO.Stock,
            Created = productDTO.Created,
            CategoryId = productDTO.CategoryId
        };
    }
}


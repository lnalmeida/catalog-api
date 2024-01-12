namespace CatalogAPI.Domain.DTO;

public class ProductDTO
{
    public ProductDTO() { }
    public ProductDTO(Guid productID, string name, string description, DateTime created,  Guid categoryId, decimal price = 0, float stock = 0 )
    {
        ProductId = productID;
        Name = name;
        Description = description;
        Created = created;
        CategoryId = categoryId;
        Price = price;
        Stock = stock;
    }
    public Guid ProductId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public float Stock { get; set;}
    public DateTime Created { get; set;}
    public Guid CategoryId { get; set;}
}

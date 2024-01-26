namespace CatalogAPI.DTO;

public class InsertProductDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public float Stock { get; set; }
    public Guid CategoryId { get; set; }
}
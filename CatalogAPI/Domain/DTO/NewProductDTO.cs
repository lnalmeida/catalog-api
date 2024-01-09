namespace CatalogAPI.Domain.DTO
{
    public class NewProductDTO
    {
        public NewProductDTO() { }
        public NewProductDTO(string name, string description, DateTime created,  Guid categoryId, decimal price = 0, float stock = 0 )
        {
            Name = name;
            Description = description;
            Created = created;
            CategoryId = categoryId;
            Price = price;
            Stock = stock;
        }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public float Stock { get; set;}
        public DateTime Created { get; set;}
        public Guid CategoryId { get; set;}
    }
}

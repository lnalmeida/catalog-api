namespace CatalogAPI.DTO
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryImageUrl { get; set; } = string.Empty;
        public IEnumerable<ProductDto> Products{ get; set; } = new List<ProductDto>();
    }
}

namespace CatalogAPI.Domain.DTO
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryImageUrl { get; set; } = string.Empty;
        public IEnumerable<ProductDTO> Products{ get; set; } = new List<ProductDTO>();

    }
}

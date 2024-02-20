using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Domain
{
    [Table("Products")]
    public class Product
    {
        public Product() {}

        public Product(string name, string description, string imageUrl, decimal price, float stock, DateTime? created, Guid categoryId)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Stock = stock;
            Created = created;
            CategoryId = categoryId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }  = Guid.NewGuid();

        [Required(ErrorMessage = "The name can not be null.")]
        [MaxLength(100)] public string Name { get; set; } = string.Empty;
        [MaxLength(350)] public string Description { get; set; } = string.Empty;
        [MaxLength(150)] public string ImageUrl { get; set; } = string.Empty;
        
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(10,2)")] 
        [Range(1, 10000, ErrorMessage = "The price should be between {1} and {2}")] 
        public decimal Price { get; set; }
        
        public float Stock { get; set; } 
        public DateTime? Created { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage ="Category Id can not be null")] 
        public Guid CategoryId { get; set; }
        
        [JsonIgnore] 
        public Category? Category { get; set; }
    }
}

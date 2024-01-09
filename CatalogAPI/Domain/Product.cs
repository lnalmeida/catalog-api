using CatalogAPI.Domain.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Domain
{
    [Table("Products")]
    public class Product
    {
        public Product() {}

        public Product(string? name, string description, decimal price, float stock, DateTime? created, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Created = created;
            CategoryId = categoryId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }  = Guid.NewGuid();

        [Required(ErrorMessage = "The name can not be null.")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(350)]
        public string? Description { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }
        public float Stock { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        [Required(ErrorMessage ="Category Id can not be null")]
        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}

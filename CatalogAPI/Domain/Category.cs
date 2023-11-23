using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Domain
{
    [Table("Categories")]
    public class Category
    {
        public Category() 
        {
            Products = new Collection<Product>();
            CategoryId = Guid.NewGuid();    
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "The name can not be null")]
        [MaxLength(80)]
        public string? CategoryName { get; set; }

        [MaxLength(300)]
        public string? CategoryImageUrl { get; set; }
        
        public ICollection<Product>? Products { get; set;}
    }
}

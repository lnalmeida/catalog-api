using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

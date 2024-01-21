using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetProductsByStock(int quantity) => await _context.Products
        .Where(p => p.Stock <= quantity)
        .OrderBy(p => p.Stock)
        .ToListAsync();
}
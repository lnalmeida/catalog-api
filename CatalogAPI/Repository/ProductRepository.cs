using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Pagination;
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

    public async Task<PagedList<Product>> GetAll(ProductsParameters productsParameters)
    {
        // return await _context.Products
        //     .OrderByDescending(on => on.Name)
        //     .AsNoTracking()
        //     .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
        //     .Take(productsParameters.PageSize)
        //     .ToListAsync();

        var allProducts = _context.Products.AsNoTracking();
        return PagedList<Product>.ToPagedList(allProducts.OrderByDescending(on => on.Name), productsParameters.PageNumber, productsParameters.PageSize);
    }

    public async Task<IEnumerable<Product>> GetProductsByStock(int quantity)
    {
        return await _context.Products
            .Where(p => p.Stock <= quantity)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    } 
}
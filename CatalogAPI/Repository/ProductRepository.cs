using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Domain.Mappers;
using CatalogAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CatalogAPI.Repository
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                if (products != null)
                {
                    return products;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred on get products. Details: {ex.Message.ToString()}");
            }
        }

        public async Task<Product> GetAsync(string id)
        {
            try
            {
                var parsedProductID = Guid.Parse(id);
                var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == parsedProductID);
                if (existingProduct != null)
                {
                    return existingProduct;
                }

                return null;               
            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred on get the product. Details: {ex.Message.ToString()}");
            }
        }

        public async Task<IEnumerable<Product>> GetByCategory(string categoryId)
        {
            try
            {
                var categoryIdGuid = Guid.Parse(categoryId);
                var productsByCategory = await _context.Products.Where(p => p.CategoryId == categoryIdGuid).ToListAsync();
                if (productsByCategory != null)
                {
                    return productsByCategory;
                }

                return null;
            } catch (Exception ex)
            {
                throw new Exception($"An error ocurred on get categoryId {categoryId} product. Details: {ex.Message.ToString()}");
            }
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            try
            {
               await _context.Products.AddAsync(entity);
               return entity;
            } catch (Exception ex)
            {
                throw new Exception($"An error ocurred on create a new product. Message: {ex.Message.ToString()}");
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var parsedId = Guid.Parse(id);
                var existingProduct = await _context.Products.FindAsync(parsedId);
                
                if (existingProduct != null)
                {
                   _context.Remove(existingProduct);
                }
                
            } catch (Exception ex)
            {
                throw new Exception($"An error ocurred on delete a product. Details: {ex.Message.ToString()}");
            }
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(entity.ProductId);
                if (existingProduct != null)
                {
                    _context.Entry(existingProduct).CurrentValues.SetValues(entity);
                    return entity;
                }

                return null;

            } catch (Exception ex)
            {
                throw new Exception($"An error ocurred on update the product. Details: {ex.Message.ToString()}");
            }
        }
    }
}

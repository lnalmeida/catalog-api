using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Domain.Mappers;
using CatalogAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Repository;

public class CategoryRepository : ICategoryRepository<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository( AppDbContext context)
    {
        _context = context;
    }
    public async  Task<IEnumerable<Category?>?> GetAllAsync()
    {
        try
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            if(categories != null)
            {
                return categories;
            }
            return null;
        } catch (Exception ex)
        {
            throw new Exception($"An error ocurred on get categories. Details: {ex.Message}");
        }
    }

    public async Task<Category?> GetAsync(string id)
    {
        try
        {
            var parsedCategoryId = Guid.Parse(id);
            var existingCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == parsedCategoryId);
            if (existingCategory != null)
            {
                return existingCategory;
            }
            return null;

        } catch (Exception ex)
        {
            throw new Exception($"An error ocurred on get the category. Details: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Category?>?> GetCategoryProducts(string categoryId)
    {
        try
        {
            var parsedCategoryId = Guid.Parse(categoryId);
            var categoryProducts = await _context.Categories
                .AsNoTracking()
                .Include(p => p.Products)
                .Where(c => c.CategoryId == parsedCategoryId)
                .ToListAsync();  
            if (categoryProducts != null)
            {
                return categoryProducts;
            }

            return null;

        } catch (Exception ex)
        {
            throw new Exception($"An error ocurred on get products by the category. Details: {ex.Message}");
        }

    }

    public async Task<Category?> CreateAsync(Category entity)
    {
        try
        {
            if (entity != null)
            {
               await _context.AddAsync(entity);
               return entity;
            }
            return null;

        } catch (Exception ex) 
        {
            throw new Exception($"An error ocurred on create the category. Details: {ex.Message}");
        }
    }

    public async Task<Category?> UpdateAsync(Category entity)
    {
        try
        {
            var existingCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == entity.CategoryId);

            if (existingCategory != null) 
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(entity);
                return entity;
            }

            return null;

        } catch (Exception ex)
        {
            throw new Exception($"An error ocurred on update the category. Details: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            var parsedCategoryId = Guid.Parse(id);
            var existingCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == parsedCategoryId);

            if (existingCategory != null)
            {
                _context.Remove(existingCategory);
            }

        } catch (Exception ex)
        {
            throw new Exception($"An error ocurred on delete the category. Details: {ex.Message}");
        }
    }
}

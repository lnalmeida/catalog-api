﻿using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Pagination;
using CatalogAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository( AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<PagedList<Category>> GetAll(PaginationParameters paginationParameters)
    {
        var allCategories =  _context.Categories.AsNoTracking();
        return await PagedList<Category>.ToPagedList(allCategories.OrderByDescending(on => on.CategoryName), paginationParameters.PageNumber, paginationParameters.PageSize);
    }
    
    public async Task<IEnumerable<Category>> GetCategoryProducts(string categoryId)
    {
        var parsedCategoryId = Guid.Parse(categoryId);
        var categoryProducts = await _context.Categories
            .AsNoTracking()
            .Include(p => p.Products)
            .Where(c => c.CategoryId == parsedCategoryId)
            .ToListAsync();
        return categoryProducts;
    }
}
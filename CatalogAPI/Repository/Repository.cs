using System.Linq.Expressions;
using CatalogAPI.Context;
using CatalogAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<T> GetAllAsync() => _context.Set<T>().AsNoTracking();
    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().SingleOrDefaultAsync(predicate);
    public void CreateAsync(T entity) =>  _context.Set<T>().Add(entity);
    public void UpdateAsync(T entity) => _context.Set<T>().Update(entity);
    public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);
}
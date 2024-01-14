using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Repository;
using CatalogAPI.Repository.Interfaces;

namespace CatalogAPI.UnityOfWork
{
    public class UnityOfWork : IUnityOfWork
    {
        //private  AppDbContext? _context;
        //private  IProductRepository<Product> _productRepository;
        //private  ICategoryRepository<Category> _categoryRepository;

        //public UnityOfWork(AppDbContext context)
        //{
        //    _context = context;
        //}

        //public IProductRepository<Product> ProductRepository
        //{
        //    get
        //    {
        //        return _productRepository ?? new ProductRepository(_context);
        //    }
        //}

        //public ICategoryRepository<Category> CategoryRepository
        //{
        //    get
        //    {
        //        return _categoryRepository ?? new CategoryRepository(_context);
        //    }
        //}

        //public async void Commit()
        //{
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    } catch (Exception ex)
        //    {
        //        throw new Exception($"An error ocurred on save changes. Details: {ex.Message}");
        //    }
        //}

        //public void Dispose()
        //{
        //    _context?.Dispose();
        //}
    }
}

using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Repository;
using CatalogAPI.Repository.Interfaces;

namespace CatalogAPI.UnityOfWork
{
    public class UnityOfWork : IUnityOfWork
    {
        private  AppDbContext _context;
        private  IProductRepository _productRepository;
        private  ICategoryRepository _categoryRepository;

        public UnityOfWork(AppDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository is null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository is null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }

                return _categoryRepository;
            }
        }

        public async Task Commit()
        { 
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

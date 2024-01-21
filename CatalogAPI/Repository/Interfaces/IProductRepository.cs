using CatalogAPI.Domain;

namespace CatalogAPI.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetProductsByStock(int quantity);
    }
}

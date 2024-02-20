using CatalogAPI.Domain;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetFullProductList();
        Task<PagedList<Product>> GetAll(PaginationParameters paginationParameter);
        Task<IEnumerable<Product>> GetProductsByStock(int quantity);
    }
}

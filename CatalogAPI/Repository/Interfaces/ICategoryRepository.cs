using CatalogAPI.Domain;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetAll(PaginationParameters paginationParameter);
        public Task<IEnumerable<Category>> GetCategoryProducts(string categoryId);
    }
}

using CatalogAPI.Domain;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetFullCategoryList();
        Task<PagedList<Category>> GetAll(PaginationParameters paginationParameter);
        Task<IEnumerable<Category>> GetCategoryProducts(string categoryId);
    }
}

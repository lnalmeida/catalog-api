using CatalogAPI.Domain;

namespace CatalogAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<IEnumerable<Category>> GetCategoryProducts(string categoryId);
    }
}

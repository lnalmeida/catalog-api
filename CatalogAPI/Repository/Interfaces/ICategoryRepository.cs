namespace CatalogAPI.Repository.Interfaces
{
    public interface ICategoryRepository<T> : IRepository<T>
    {
        public Task<IEnumerable<T>> GetCategoryProducts(string categoryId);
    }
}

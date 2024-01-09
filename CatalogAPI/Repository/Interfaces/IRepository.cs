namespace CatalogAPI.Repository.Interfaces
{
    public interface IRepository<T>
    {
        
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetAsync(string id);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task DeleteAsync(string id);
    }
}

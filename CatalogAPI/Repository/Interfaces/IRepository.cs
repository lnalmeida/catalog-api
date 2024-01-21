using System.Linq.Expressions;

namespace CatalogAPI.Repository.Interfaces
{
    public interface IRepository<T>
    {
        
        IQueryable<T> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
    }
}

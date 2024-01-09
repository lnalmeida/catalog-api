using CatalogAPI.Domain.DTO;
using System.Security.Claims;

namespace CatalogAPI.Repository.Interfaces
{
    public interface IProductRepository<T> : IRepository<T>
    {
        public Task<IEnumerable<T>> GetByCategory(string categoryId);
    }
}

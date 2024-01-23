﻿using CatalogAPI.Domain;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetAll(PaginationParameters paginationParameter);
        Task<IEnumerable<Product>> GetProductsByStock(int quantity);
    }
}

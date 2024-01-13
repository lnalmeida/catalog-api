﻿using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Repository.Interfaces;

namespace CatalogAPI.UnityOfWork
{
    public interface IUnityOfWork : IDisposable
    {
        IProductRepository<Product> ProductRepository { get; }
        ICategoryRepository<Category> CategoryRepository { get; }
        void Commit();
    }
}
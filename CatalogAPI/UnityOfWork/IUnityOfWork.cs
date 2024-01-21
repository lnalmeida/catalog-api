using CatalogAPI.Repository.Interfaces;

namespace CatalogAPI.UnityOfWork
{
    public interface IUnityOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task Commit();
    }
}

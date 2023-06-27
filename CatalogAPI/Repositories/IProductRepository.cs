using CatalogAPI.Models;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetProducts(ProductsParameters parameters);
        Task<IEnumerable<Product>> GetProductsPerPrice();
    }
}

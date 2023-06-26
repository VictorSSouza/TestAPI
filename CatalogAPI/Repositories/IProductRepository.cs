using CatalogAPI.Models;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        PagedList<Product> GetProducts(ProductsParameters parameters);
        IEnumerable<Product> GetProductsPerPrice();
    }
}

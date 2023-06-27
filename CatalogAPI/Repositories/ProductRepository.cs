using CatalogAPI.Data;
using CatalogAPI.Models;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogAppDbContext context) : base(context)
        {
        }
        public PagedList<Product> GetProducts(ProductsParameters parameters)
        {
            /*return Get()
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList(); */
            return PagedList<Product>.ToPagedList(Get().OrderBy(x => x.Name), parameters.PageSize, parameters.PageNumber);

        }
        public IEnumerable<Product> GetProductsPerPrice()
        {
            return Get().OrderBy(x => x.Price).ToList();
        }
    }
}

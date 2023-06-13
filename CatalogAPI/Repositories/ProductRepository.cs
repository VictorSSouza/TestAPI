using CatalogAPI.Data;
using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogAppDbContext context) : base(context)
        {
        }
        public IEnumerable<Product> GetProductsPerPrice()
        {
            return Get().OrderBy(x => x.Price).ToList();
        }
    }
}

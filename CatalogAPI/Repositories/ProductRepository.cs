using CatalogAPI.Data;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogAppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<Product>> GetProducts(ProductsParameters parameters)
        {
            /*return Get()
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList(); */
            return await PagedList<Product>.ToPagedList(Get().OrderBy(x => x.Name), parameters.PageSize, parameters.PageNumber);

        }
        public async Task<IEnumerable<Product>> GetProductsPerPrice()
        {
            return await Get().OrderBy(x => x.Price).ToListAsync();
        }
    }
}

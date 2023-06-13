using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogAppDbContext context) : base(context)
        {
        }
        public IEnumerable<Category> GetCategoriesProducts()
        {
            return Get().Include(x => x.Products);
        }
    }
}

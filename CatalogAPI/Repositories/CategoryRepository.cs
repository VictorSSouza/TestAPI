using CatalogAPI.Data;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogAppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<Category>> GetCategories(CategoriesParameters parameters)
        {
            return await PagedList<Category>.ToPagedList(Get().OrderBy(x => x.Id), parameters.PageSize, parameters.PageNumber);
        }
        public async Task<IEnumerable<Category>> GetCategoriesProducts()
        {
            return await Get().Include(x => x.Products).Where(c => c.Id <= 5).ToListAsync();
        }
    }
}

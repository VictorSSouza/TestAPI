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
        public PagedList<Category> GetCategories(CategoriesParameters parameters)
        {
            return PagedList<Category>.ToPagedList(Get().OrderBy(x => x.Name), parameters.PageSize, parameters.PageNumber);
        }
        public IEnumerable<Category> GetCategoriesProducts()
        {
            return Get().Include(x => x.Products).Where(c => c.Id <= 5);
        }
    }
}

using CatalogAPI.Models;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetCategories(CategoriesParameters parameters);
        Task<IEnumerable<Category>> GetCategoriesProducts();
    }
}
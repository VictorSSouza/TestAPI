﻿using CatalogAPI.Models;
using CatalogAPI.Pagination;

namespace CatalogAPI.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        PagedList<Category> GetCategories(CategoriesParameters parameters);
        IEnumerable<Category> GetCategoriesProducts();
    }
}
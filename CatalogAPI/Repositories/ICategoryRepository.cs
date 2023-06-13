﻿using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoriesProducts();
    }
}

using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data;
public class CatalogAppDbContext : DbContext
{
    public CatalogAppDbContext(DbContextOptions<CatalogAppDbContext> options) : base(options)
    {
    }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }
}


using CatalogAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data;
public class CatalogAppDbContext : IdentityDbContext
{
    public CatalogAppDbContext(DbContextOptions<CatalogAppDbContext> options) : base(options)
    {
    }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }
}


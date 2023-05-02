using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(300)]
    public string? ImageUrl { get; set; }
    public ICollection<Product>? Products { get; set; }

    public Category()
    {
        Products = new Collection<Product>();
    }
}


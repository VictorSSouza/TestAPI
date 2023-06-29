using CatalogAPI.Validations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage="O nome é obrigatório")]
    [StringLength(100, ErrorMessage="O nome deve ter no máximo {1} e no mínimo {2} caracteres", MinimumLength = 3)]
    public string? Name { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImageUrl { get; set; }
    public ICollection<Product>? Products { get; set; }

    public Category()
    {
        Products = new Collection<Product>();
    }
}


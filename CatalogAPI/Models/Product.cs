using CatalogAPI.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage="O nome é obrigatório")]
        [StringLength(100, ErrorMessage="O nome deve ter no máximo {1} e no mínimo {2} caracteres", MinimumLength = 5)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string? Description { get; set; }

        [Required]
	[DataType(DataType.Currency)]
        [Column(TypeName ="decimal(10,2)")]
	[Range(1, 100000, ErrorMessage="O preço deve estar entre {1} e {2}")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImageUrl { get; set; }

        public float Amount { get; set; }
        public DateTime DateRegistration { get; set; }
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }
        
    }
}

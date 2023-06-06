using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models
{
    public class Product : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }
        public float Amount { get; set; }
        public DateTime DateRegistration { get; set; }
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                string firstLetter = this.Name[0].ToString();
                if(firstLetter != firstLetter.ToUpper())
                {
                    yield return new
                        ValidationResult("A primeira letra do produto deve ser maiúscula!",
                        new[] { nameof(this.Name) }
                        );
                }
            }

            if(this.Amount <= 0)
            {
                yield return new
                        ValidationResult("O estoque do produto deve ser maior que zero!",
                        new[] { nameof(this.Amount) }
                        );
            }
        }
    }
}

using CatalogAPI.Validations;

namespace CatalogAPI.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [FirstLetterUpper]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}

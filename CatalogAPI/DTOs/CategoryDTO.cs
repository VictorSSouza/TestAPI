﻿namespace CatalogAPI.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProductDTO>? Products { get; set; }
    }
}

using CatalogAPI.Data;
using CatalogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogAPIxUnitTests
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {
        }

        //metodo para inserir dados nas tabelas do Banco de Dados
        public void Seed(CatalogAppDbContext context)
        {
            context.Categories.Add
                (new Category { Id = 1, Name = "Bebidas 1000-7", ImageUrl = "bebidas993.png" });
            context.Categories.Add
                (new Category { Id = 2, Name = "Sorvetes", ImageUrl = "sorvetes1.png" });
            context.Categories.Add
                (new Category { Id = 3, Name = "Doces", ImageUrl = "doces1.png" });
            context.Categories.Add
                (new Category { Id = 4, Name = "Lanches", ImageUrl = "lanches2.png" });
            context.Categories.Add
                (new Category { Id = 5, Name = "Bolos", ImageUrl = "bolos1.png" });
            context.Categories.Add
                (new Category { Id = 6, Name = "Petiscos", ImageUrl = "petiscos1.png" });
            context.Categories.Add
                (new Category { Id = 7, Name = "Churrascos", ImageUrl = "churrascos1.png" });
            context.Categories.Add
                (new Category { Id = 8, Name = "Sucos", ImageUrl = "sucos1.png" });
            context.Categories.Add
                (new Category { Id = 9, Name = "Tortas", ImageUrl = "tortas1.png" });

            context.Products.Add
            (new Product
            {
                Id = 1,
                Name = "Cafeeeeeé",
                Description = "café puro",
                ImageUrl = "cafe1.png",
                Price = 7,
                Amount = 100,
                DateRegistration = DateTime.Now
            });
            context.Products.Add
            (new Product
            {
                Id = 10,
                Name = "Cervejinha",
                Description = "cerveja no copo",
                ImageUrl = "cervejacopo1.png",
                Price = 11,
                Amount = 500,
                DateRegistration = DateTime.Now,
                CategoryId = 993
            });
            context.Products.Add
            (new Product
            {
                Id = 2,
                Name = "Sorvete na casquinha",
                Description = "sorvete na casquinha comestivel",
                ImageUrl = "sorvetecosquinha.png",
                Price = 8,
                Amount = 90,
                DateRegistration = DateTime.Now,
                CategoryId = 2
            });
            context.Products.Add
            (new Product
            {
                Id = 3,
                Name = "Sorvete na taça",
                Description = "sorvete na taça de vidro",
                ImageUrl = "sorvetetaca.png",
                Price = 11,
                Amount = 70,
                DateRegistration = DateTime.Now,
                CategoryId = 2
            });
            context.Products.Add
            (new Product
            {
                Id = 4,
                Name = "Bolo de morango",
                Description = "bolo de morango hummmmm",
                ImageUrl = "bolomorango.png",
                Price = 70,
                Amount = 10,
                DateRegistration = DateTime.Now,
                CategoryId = 5
            });
            context.Products.Add
            (new Product
            {
                Id = 5,
                Name = "Bolo de cenoura",
                Description = "bolo de cenoura com cobertura de chocolate",
                ImageUrl = "bolocenoura.png",
                Price = 70,
                Amount = 8,
                DateRegistration = DateTime.Now,
                CategoryId = 5
            });
            context.Products.Add
            (new Product
            {
                Id = 6,
                Name = "Pastel de frango",
                Description = "pastel de frango hummmmm",
                ImageUrl = "pastelfrango.png",
                Price = 6,
                Amount = 40,
                DateRegistration = DateTime.Now,
                CategoryId = 4
            });

            context.SaveChanges();
        }
    }
}

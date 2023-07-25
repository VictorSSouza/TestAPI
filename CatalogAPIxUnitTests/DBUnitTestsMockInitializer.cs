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
                (new Category{ Id = 993, Name = "Bebidas 1000-7", ImageUrl = "bebidas993.png" });
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

            context.SaveChanges();
        }
    }
}

using AutoMapper;
using CatalogAPI.Controllers;
using CatalogAPI.Data;
using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogAPIxUnitTests
{
    public class CategoriesUnitTestController
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public static DbContextOptions<CatalogAppDbContext> dbContextOptions { get;}

        public static string connectionString =
            "Server=localhost;Database=Catalog_app;Uid=DevVictor;Pwd=1234567";

        static CategoriesUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<CatalogAppDbContext>()
                                .UseMySql(connectionString,
                                    ServerVersion.AutoDetect(connectionString))
                                .Options;
        }

        public CategoriesUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile());
                });

            _mapper = config.CreateMapper();

            var context = new CatalogAppDbContext(dbContextOptions);

            // Utilizar para preencher banco de dados para testes
            // DbUnitTestsMocksInitializer db = new DbUnitTestsMocksInitializer();
            // db.Seed(context)

            _context = new UnitOfWork(context);
        }

        // Testes Unitários
        // Método GET
        // Para realizar o teste é preciso comentar '//' em CategoriesController line 86 
        [Fact]
        public async void GetCategories_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);

            CategoriesParameters parameters = new CategoriesParameters() 
            {
                PageNumber= 1,
                PageSize= 10
            };
            // Act = Execucao
            var data = await controller.Get(parameters);
            // Assert = verificacao do retorno
            Assert.IsType<List<CategoryDTO>>(data.Value);
        }

    }
}

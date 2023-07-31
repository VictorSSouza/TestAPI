using AutoMapper;
using CatalogAPI.Controllers;
using CatalogAPI.Data;
using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogAPIxUnitTests
{
    public class ProductsUnitTestController
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public static DbContextOptions<CatalogAppDbContext> dbContextOptions { get;}

        public static string connectionString =
            "Server=localhost;Database=Catalog_app;Uid=DevVictor;Pwd=1234567";

        static ProductsUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<CatalogAppDbContext>()
                                .UseMySql(connectionString,
                                    ServerVersion.AutoDetect(connectionString))
                                .Options;
        }

        public ProductsUnitTestController()
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
        // Método GetProductsPerPrice
        // GETProductsPerPrice - OkResult
        [Fact]
        public async void GetProductsPerPrice_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            // Act = Execucao
            var data = await controller.GetProductsPerPrice();
            // Assert = verificacao do retorno
            Assert.IsType<List<ProductDTO>>(data.Value);
        }

        // GETProductsPerPrice - StatusCodeResult
        [Fact]
        public async void GetProductsPerPrice_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            // Act = Execucao
            var data = await controller.GetProductsPerPrice();
            // Assert = verificacao do retorno
            Assert.IsType<ObjectResult>(data.Result);
        }
        // Método GET
        // Para realizar os testes GET é necessário comentar '//' em ProductsController line 77 
        [Fact]
        public async void GetProducts_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);

            ProductsParameters parameters = new ProductsParameters() 
            {
                PageNumber= 1,
                PageSize= 10
            };
            // Act = Execucao
            var data = await controller.Get(parameters);
            // Assert = verificacao do retorno
            Assert.IsType<List<ProductDTO>>(data.Value);
        }

        // GET - StatusCode
        [Fact]
        public async void GetProducts_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);

            ProductsParameters parameters = new ProductsParameters()
            {
                PageNumber = 1,
                PageSize = 10
            };
            // Act = Execucao
            var data = await controller.Get(parameters);
            // Assert = verificacao do retorno
            Assert.IsType<ObjectResult>(data.Result);
        }

        // GET - MatchResult
        [Fact]
        public async void GetProducts_Return_MatchResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);

            ProductsParameters parameters = new ProductsParameters()
            {
                PageNumber = 1,
                PageSize = 10
            };
            // Act = Execucao
            var data = await controller.Get(parameters);
            // Assert = verificacao do retorno
            Assert.IsType<List<ProductDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<ProductDTO>>().Subject;

            Assert.Equal("Coca-Cola Diet", cat[0].Name);
            Assert.Equal("Refrigerante de Cola 350 ml", cat[0].Description);
            Assert.Equal("cocacola.jpg", cat[0].ImageUrl);

            Assert.Equal("Pudim 100g", cat[2].Name);
            Assert.Equal("Pudim de leite condensado 100g", cat[2].Description);
            Assert.Equal("pudim.jpg", cat[2].ImageUrl);
        }

        // Método Get por Id
        [Fact]
        public async void GetProductById_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var catId = 1;
            // Act = Execucao
            var data = await controller.GetProduct(catId);
            // Assert = verificacao do retorno
            Assert.IsType<ProductDTO>(data.Value);
        }

        // GET por Id - NotFound
        [Fact]
        public async void GetProductById_Return_NotFound()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var catId = 109;
            // Act = Execucao
            var data = await controller.GetProduct(catId);
            // Assert = verificacao do retorno
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        // Get por Id - StatusCodeResult
        [Fact]
        public async void GetProductById_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var catId = 2;
            // Act = Execucao
            var data = await controller.GetProduct(catId);
            // Assert = verificacao do retorno
            Assert.IsType<ObjectResult>(data.Result);
        }

        // Método Post
        [Fact]
        public async void PostProduct_Return_CreatedResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var cat = new ProductDTO()
            {
                Name = "Teste unitário post",
                Description = "Testando blabla",
                Price = 100,
                ImageUrl = "teste.png",
                CategoryId = 1
            };

            // Act = Execucao
            var data = await controller.Post(cat);
            // Assert = verificacao do retorno
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        // Método Put
        // Infelizmente não consegui fazer de outra maneira :(
        [Fact]
        public async void PutProduct_ValidData_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var catId = 11;

            // Act = Execucao
            var existingPost = await controller.GetProduct(catId);
            var result = existingPost.Value.Should().BeAssignableTo<ProductDTO>().Subject;

            var catDTO = new ProductDTO();
            catDTO.Id = catId;
            catDTO.Name = "Teste unitário alterado 1";
            catDTO.Description = result.Description;
            catDTO.Price = 99;
            catDTO.ImageUrl = result.ImageUrl;
            catDTO.CategoryId = result.CategoryId;

            var updatedData = await controller.Put(catId, catDTO);
            // Assert = verificacao do retorno
            Assert.IsType<OkObjectResult>(updatedData);
        }

        // Método Delete por Id
        [Fact]
        public async void DeleteProduct_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new ProductsController(_context, _mapper);
            var catId = 11;
            // Act = Execucao
            var data = await controller.Delete(catId);
            // Assert = verificacao do retorno
            Assert.IsType<OkObjectResult>(data);
        }
    }
}

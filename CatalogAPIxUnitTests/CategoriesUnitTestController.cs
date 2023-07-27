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
        // Método GetCategoriesProducts
        // GETCategoriesProducts - OkResult
        [Fact]
        public async void GetCategoriesProducts_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            // Act = Execucao
            var data = await controller.GetCategoriesProducts();
            // Assert = verificacao do retorno
            Assert.IsType<List<CategoryDTO>>(data.Value);
        }

        // GETCategoriesProducts - StatusCodeResult
        [Fact]
        public async void GetCategoriesProducts_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            // Act = Execucao
            var data = await controller.GetCategoriesProducts();
            // Assert = verificacao do retorno
            Assert.IsType<ObjectResult>(data.Result);
        }
        // Método GET
        // Para realizar os testes GET é necessário comentar '//' em CategoriesController line 86 
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

        // GET - StatusCode
        [Fact]
        public async void GetCategories_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);

            CategoriesParameters parameters = new CategoriesParameters()
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
        public async void GetCategories_Return_MatchResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);

            CategoriesParameters parameters = new CategoriesParameters()
            {
                PageNumber = 1,
                PageSize = 10
            };
            // Act = Execucao
            var data = await controller.Get(parameters);
            // Assert = verificacao do retorno
            Assert.IsType<List<CategoryDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoryDTO>>().Subject;

            Assert.Equal("Bebidas", cat[0].Name);
            Assert.Equal("bebidas.jpg", cat[0].ImageUrl);

            Assert.Equal("Sobremesas", cat[2].Name);
            Assert.Equal("sobremesas.jpg", cat[2].ImageUrl);
        }

        // Método Get por Id
        [Fact]
        public async void GetCategoryById_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var catId = 1;
            // Act = Execucao
            var data = await controller.GetCategory(catId);
            // Assert = verificacao do retorno
            Assert.IsType<OkObjectResult>(data.Result);
        }

        // GET por Id - NotFound
        [Fact]
        public async void GetCategoryById_Return_NotFound()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var catId = 109;
            // Act = Execucao
            var data = await controller.GetCategory(catId);
            // Assert = verificacao do retorno
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        // Get por Id - StatusCodeResult
        [Fact]
        public async void GetCategoryById_Return_StatusCodeResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var catId = 2;
            // Act = Execucao
            var data = await controller.GetCategory(catId);
            // Assert = verificacao do retorno
            Assert.IsType<ObjectResult>(data.Result);
        }

        // Método Post
        [Fact]
        public async void PostCategory_Return_CreatedResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var cat = new CategoryDTO()
            {
                Name = "Teste unitário post",
                ImageUrl = "teste.png"
            };

            // Act = Execucao
            var data = await controller.Post(cat);
            // Assert = verificacao do retorno
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        // Método Put
        /* Para realizar os testes PUT é necessário comentar '//' em CategoriesController line 119 
            e remover '//' em CategoriesController line 120 
            trocando 'return Ok(categoryDTO);' por 'return CategoryDTO;'
         */
        // Infelizmente não consegui fazer de outra maneira :(
        [Fact]
        public async void PutCategory_ValidData_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var catId = 8;

            // Act = Execucao
            var existingPost = await controller.GetCategory(catId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoryDTO>().Subject;

            var catDTO = new CategoryDTO();
            catDTO.Id = catId;
            catDTO.Name = "Teste unitário alterado 1";
            catDTO.ImageUrl = result.ImageUrl;

            var updatedData = await controller.Put(catId, catDTO);
            // Assert = verificacao do retorno
            Assert.IsType<OkObjectResult>(updatedData);
        }

        // Método Delete por Id
        [Fact]
        public async void DeleteCategory_Return_OkResult()
        {
            // Arrange == preparacao
            var controller = new CategoriesController(_context, _mapper);
            var catId = 10;
            // Act = Execucao
            var data = await controller.Delete(catId);
            // Assert = verificacao do retorno
            Assert.IsType<OkObjectResult>(data);
        }
    }
}

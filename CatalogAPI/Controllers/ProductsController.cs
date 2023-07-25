using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogAPI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    // para ativar o cors comente a linha abaixo com '//' antes do primeiro '['
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
            _mapper = mapper;
        }

	    /// <summary>
	    /// Retorna uma lista de produtos em ordem crescente
	    /// </summary>
	    /// <returns>Lista de produtos</returns>
        [HttpGet("ordempreco")]
	    //[EnableCors("PermitirApiRequest")] // request apenas nesse metodo
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsPerPrice()
        {
            var products = await _uow.ProductRepository.GetProductsPerPrice();
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }

	    /// <summary>
	    /// Retorna uma lista de produtos
	    /// </summary>
	    /// <returns>Lista de objetos Produtos</returns>
        [HttpGet]
        //[ServiceFilter(typeof(APILoggingFilter))] // ativacao de servico para registrar login do metodo Get
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters parameters)
        {
            try
            {
                var products = await _uow.ProductRepository.GetProducts(parameters);

                if (products is null)
                {
                    return NotFound("Produtos não localizados!");
                }
                // dados anonimizados para criar uma response customizada no header para paginacao
                var metadata = new
                {
                    products.TotalCount,
                    products.PageSize,
                    products.CurrentPage,
                    products.TotalPages,
                    products.HasNext,
                    products.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

                var productsDTO = _mapper.Map<List<ProductDTO>>(products);
                return productsDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }

        }

	    /// <summary>
	    /// Retorna um produto
	    /// </summary>
	    /// <param name ="id">Codigo do Produto</param>
	    /// <returns>Objeto Produto</returns>
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _uow.ProductRepository.GetById(x => x.Id == id);

                if (product is null)
                {
                    return NotFound("Produto não encontrado!");
                }

                var productDTO = _mapper.Map<ProductDTO>(product);

                return productDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        /// <summary>
        /// Adicionar um produto
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///	Post /Products
        ///	{
        ///	    "id" : 1,
        ///	    "name" : "Produto de teste",
        ///	    "description" : "descrição do produto",
        ///	    "price" : 10.00,
        ///	    "imageUrl" : "foto_produto.png",
        ///	    "categoryId" : 1
        ///	}
        /// </remarks>
        /// <param name ="productDTO">Objeto Produto</param>
        /// <returns>Objeto Produto adicionado</returns>
        /// <remarks>Retorna o objeto Produto adicionado</remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO is null)
                {
                    return BadRequest();
                }

                var product = _mapper.Map<Product>(productDTO);

                _uow.ProductRepository.Add(product);
                await _uow.Commit();

                var productDto = _mapper.Map<ProductDTO>(product);

                return new CreatedAtRouteResult("ObterProduto", new { id = product.Id }, productDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        /// <summary>
        /// Modificar um produto
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///	Put /Products
        ///	{
        ///	    "name" : "Produto alterado",
        ///	    "description" : "descrição do produto alterado",
        ///	    "price" : 9.90,
        ///	    "imageUrl" : "foto_produto(2).png",
        ///	    "categoryId" : 1
        ///	}
        /// </remarks>
        /// <returns>Status 200 e string</returns>
        /// <remarks>Retorna status 200 e messagem string Produto modificado</remarks>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                if (id != productDTO.Id)
                {
                    return BadRequest();
                }

                var product = _mapper.Map<Product>(productDTO);

                _uow.ProductRepository.Update(product);
                await _uow.Commit();

                return Ok("Produto modificado");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

	    /// <summary>
	    /// Excluir um produto
	    /// </summary>
	    /// <param name ="id">Codigo do Produto</param>
	    /// <returns>Status 200 e objeto Produto excluido</returns>
	    /// <remarks>Retorna status 200 e objeto Produto excluido</remarks>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var product = await _uow.ProductRepository.GetById(x => x.Id == id);

                if (product is null)
                {
                    return NotFound("Produto não localizado!");
                }

                _uow.ProductRepository.Delete(product);
                await _uow.Commit();

                var productDTO = _mapper.Map<ProductDTO>(product);
                return Ok(productDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }
    }
}

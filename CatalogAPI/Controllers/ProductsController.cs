using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
            _mapper = mapper;
        }

        [HttpGet("ordempreco")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsPerPrice()
        {
            var products = _uow.ProductRepository.GetProductsPerPrice().ToList();
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }

        [HttpGet]
        //[ServiceFilter(typeof(APILoggingFilter))]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters parameters)
        {
            try
            {
                var products = _uow.ProductRepository.GetProducts(parameters);

                if (products is null)
                {
                    return NotFound("Produtos não localizados!");
                }

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

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            try
            {
                var product = _uow.ProductRepository.GetById(x => x.Id == id);

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

        [HttpPost]
        public ActionResult Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO is null)
                {
                    return BadRequest();
                }

                var product = _mapper.Map<Product>(productDTO);

                _uow.ProductRepository.Add(product);
                _uow.Commit();

                var productDto = _mapper.Map<ProductDTO>(product);

                return new CreatedAtRouteResult("ObterProduto", new { id = product.Id }, productDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                if (id != productDTO.Id)
                {
                    return BadRequest();
                }

                var product = _mapper.Map<Product>(productDTO);

                _uow.ProductRepository.Update(product);
                _uow.Commit();

                return Ok();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _uow.ProductRepository.GetById(x => x.Id == id);

                if (product is null)
                {
                    return NotFound("Produto não localizado!");
                }

                _uow.ProductRepository.Delete(product);
                _uow.Commit();

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

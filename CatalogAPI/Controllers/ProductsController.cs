using CatalogAPI.Models;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public ProductsController(IUnitOfWork context)
        {
            _uow = context;
        }

        [HttpGet("ordempreco")]
        public ActionResult<IEnumerable<Product>> GetProductsPerPrice() 
        {
            return _uow.ProductRepository.GetProductsPerPrice().ToList();
        }
        
        [HttpGet]
        //[ServiceFilter(typeof(APILoggingFilter))]
        public  ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var products =  _uow.ProductRepository.Get().ToList();

                if (products is null)
                {
                    return NotFound("Produtos não localizados!");
                }
                return products;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public  ActionResult<Product> GetProduct(int id)
        {
            try
            {
                var product =  _uow.ProductRepository.GetById(x => x.Id == id);

                if (product is null)
                {
                    return NotFound("Produto não encontrado!");
                }
                return product;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpPost]
        public  ActionResult Post([FromBody] Product product)
        {
            try
            {
                if (product is null)
                {
                    return BadRequest();
                }

                _uow.ProductRepository.Add(product);
                _uow.Commit();

                return new CreatedAtRouteResult("ObterProduto", new { id = product.Id }, product);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpPut("{id:int}")]
        public  ActionResult Put(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }

                _uow.ProductRepository.Update(product);
                _uow.Commit();

                return Ok(product);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpDelete("{id:int}")]
        public  ActionResult Delete(int id)
        {
            try
            {
                var produto = _uow.ProductRepository.GetById(x => x.Id == id);

                if (produto is null)
                {
                    return NotFound("Produto não localizado!");
                }

                _uow.ProductRepository.Delete(produto);
                _uow.Commit();

                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }
    }
}

using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogAppDbContext _context;
        public ProductsController(CatalogAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();

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
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

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
        public async Task<ActionResult> Post(Product product)
        {
            try
            {
                if (product is null)
                {
                    return BadRequest();
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto", new { id = product.Id }, product);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (produto is null)
                {
                    return NotFound("Produto não localizado!");
                }

                _context.Products.Remove(produto);
                await _context.SaveChangesAsync();

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

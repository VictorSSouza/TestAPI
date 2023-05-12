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
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context.Products.ToList();

            if(products is null)
            {
                return NotFound("Produtos não localizados!");
            }
            return products;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product =_context.Products.FirstOrDefault(p => p.Id == id);
            
            if(product is null)
            {
                return NotFound("Produto não encontrado!");
            }
            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if(product is null)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new {id = product.Id}, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Products.FirstOrDefault(x => x.Id == id);

            if(produto is null)
            {
                return NotFound("Produto não localizado!");
            }

            _context.Products.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}

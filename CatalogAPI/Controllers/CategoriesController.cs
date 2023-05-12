using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogAppDbContext _context;
        public CategoriesController(CatalogAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            return _context.Categories
                .Include(c => c.Products)
                .ToList();
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            return _context.Categories.ToList();
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if(category == null)
            {
                return NotFound("Categoria não encontrada!");
            }

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if(category == null)
            {
                return BadRequest();
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id,Category category)
        {
            if(id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category =_context.Categories.FirstOrDefault(x => x.Id == id);

            if(category == null )
            {
                return NotFound("Categoria não localizada!");
            }
            
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}

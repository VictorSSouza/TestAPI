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

            try
            {
                return _context.Categories
                .Include(c => c.Products)
                .Where(c => c.Id <= 5)
                .ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            try
            {
                return _context.Categories.AsNoTracking().ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Category> GetCategory(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não encontrada!");
                }

                return Ok(category);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                _context.Categories.Add(category);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, category);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id,Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("Dados inválidos.");
                }

                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(category);
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
                var category = _context.Categories.FirstOrDefault(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não localizada!");
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
    }
}

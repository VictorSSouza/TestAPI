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
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProducts()
        {

            try
            {
                return await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.Id <= 5)
                .ToListAsync();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            try
            {
                return await _context.Categories.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category =  await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

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
        public async Task<ActionResult> Post(Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, category);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("Dados inválidos.");
                }

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(category);
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
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não localizada!");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

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

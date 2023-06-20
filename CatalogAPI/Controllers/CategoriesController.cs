using CatalogAPI.Models;
using CatalogAPI.Repositories;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public CategoriesController(IUnitOfWork context)
        {
            _uow = context;
        }

        [HttpGet("{name}")]
        public ActionResult<string> GetGreeting([FromServices] IMyService myService, string name)
        {
            return myService.Greeting(name);
        }

        [HttpGet("produtos")]
        public  ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {

            try
            {
                return  _uow.CategoryRepository
                .GetCategoriesProducts()
                .ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
        
        [HttpGet]
        public  ActionResult<IEnumerable<Category>> Get()
        {
            try
            {
                return  _uow.CategoryRepository.Get().ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public  ActionResult<Category> GetCategory(int id)
        {
            try
            {
                var category =   _uow.CategoryRepository.GetById(x => x.Id == id);

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
        public  ActionResult Post([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                _uow.CategoryRepository.Add(category);
                _uow.Commit();

                return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, category);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPut("{id:int}")]
        public  ActionResult Put(int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("Dados inválidos.");
                }

                _uow.CategoryRepository.Update(category);
                _uow.Commit();

                return Ok(category);
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
                var category =  _uow.CategoryRepository.GetById(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não localizada!");
                }

                _uow.CategoryRepository.Delete(category);
                _uow.Commit();

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

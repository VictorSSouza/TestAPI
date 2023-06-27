using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
	    private readonly IMapper _mapper;
        public CategoriesController(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
	        _mapper = mapper;
        }

        [HttpGet("{name}")]
        public ActionResult<string> GetGreeting([FromServices] IMyService myService, string name)
        {
            return myService.Greeting(name);
        }

        [HttpGet("produtos")]
        public  ActionResult<IEnumerable<CategoryDTO>> GetCategoriesProducts()
        {

            try
            {
                var categoriesP = _uow.CategoryRepository.GetCategoriesProducts().ToList();
		        var categoriesPDTO = _mapper.Map<List<CategoryDTO>>(categoriesP);
		
		        return categoriesPDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
        
        [HttpGet]
        public  ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters parameters)
        {
            try
            {
		        var categories = _uow.CategoryRepository.GetCategories(parameters);

                var metadata = new
                {
                    categories.TotalCount,
                    categories.PageSize,
                    categories.CurrentPage,
                    categories.TotalPages,
                    categories.HasNext,
                    categories.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

		        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);

                return categoriesDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public  ActionResult<CategoryDTO> GetCategory(int id)
        {
            try
            {
                var category =   _uow.CategoryRepository.GetById(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não encontrada!");
                }
		
		        var categoryDTO = _mapper.Map<CategoryDTO>(category);
                return Ok(categoryDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPost]
        public  ActionResult Post([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest("Dados inválidos.");
                }
		
		        var category = _mapper.Map<Category>(categoryDTO);
                _uow.CategoryRepository.Add(category);
                _uow.Commit();

		        var categoryDto = _mapper.Map<CategoryDTO>(category);

                return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, categoryDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPut("{id:int}")]
        public  ActionResult Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (id != categoryDTO.Id)
                {
                    return BadRequest("Dados inválidos.");
                }

		        var category = _mapper.Map<Category>(categoryDTO);
                _uow.CategoryRepository.Update(category);
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
		
		        var categoryDTO = _mapper.Map<CategoryDTO>(category);
                return Ok(categoryDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
    }
}

using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogAPI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    // para ativar o cors comente a linha abaixo com '//' antes do primeiro '['
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    //[EnableCors("PermitirApiRequest")] // request em todos os metodos get 
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

	    /// <summary>
	    /// Retorna uma lista de categorias com produtos
	    /// </summary>
	    /// <returns>lista de objetos Categorias incluindo os objetos Produtos</returns>  
        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
        {

            try
            {
                var categoriesP = await _uow.CategoryRepository.GetCategoriesProducts();
		        var categoriesPDTO = _mapper.Map<List<CategoryDTO>>(categoriesP);
		
		        return categoriesPDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }
 
	    /// <summary>
	    /// Retorna uma lista de categorias
	    /// </summary>
	    /// <returns>lista de objetos Categorias</returns>       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters parameters)
        {
            try
            {
		        var categories = await _uow.CategoryRepository.GetCategories(parameters);
			    if(categories is null)
			    {
				    return NotFound("Categorias não localizadas!");
			    }
                // dados anonimizados para criar uma response customizada no header para paginacao
                var metadata = new
                {
                    categories.TotalCount,
                    categories.PageSize,
                    categories.CurrentPage,
                    categories.TotalPages,
                    categories.HasNext,
                    categories.HasPrevious
                };
                // Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                
		        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
                return categoriesDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

	    /// <summary>
	    /// Retorna uma categoria
	    /// </summary>
	    /// <param name ="id">Codigo da Categoria</param>
	    /// <returns>Objeto Categoria</returns>
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        //[EnableCors("PermitirApiRequest")] // request mais especifico, apenas uma categoria 
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await _uow.CategoryRepository.GetById(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não encontrada!");
                }
		
		        var categoryDTO = _mapper.Map<CategoryDTO>(category);
                return Ok(categoryDTO);
                //return categoryDTO;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

	    /// <summary>
	    /// Adicionar uma categoria
	    /// </summary>
	    /// <remarks>
	    /// Exemplo de request:
	    ///	Post /Categories
	    ///	{
	    ///	    "id" : 1,
	    ///	    "name" : "Categoria de teste",
	    ///	    "imageUrl" : "foto_categoria.png"
	    ///	}
	    /// </remarks>
	    /// <param name ="categoryDTO">Objeto Categoria</param>
	    /// <returns>Objeto Categoria adicionado</returns>
	    /// <remarks>Retorna objeto Categoria adicionado</remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest("Dados inválidos.");
                }
		
		        var category = _mapper.Map<Category>(categoryDTO);
                _uow.CategoryRepository.Add(category);
                await _uow.Commit();
		        var categoryDto = _mapper.Map<CategoryDTO>(category);

                return new CreatedAtRouteResult("ObterCategoria", new { id = category.Id }, categoryDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

	    /// <summary>
	    /// Modificar uma categoria
	    /// </summary>
	    /// <remarks>
	    /// Exemplo de request:
	    ///	Put /Categories
	    ///	{
	    ///	    "name" : "Categoria alterada",
	    ///	    "imageUrl" : "foto_categoria.png"
	    ///	}
	    /// </remarks>
	    /// <returns>Status 200 e objeto Categoria modificado</returns>
	    /// <remarks>Retorna status 200 e objeto Categoria modificado</remarks>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (id != categoryDTO.Id)
                {
                    return BadRequest("Dados inválidos.");
                }

		        var category = _mapper.Map<Category>(categoryDTO);
                _uow.CategoryRepository.Update(category);
                await _uow.Commit();

                return Ok("Categoria modificada");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

	    /// <summary>
	    /// Excluir uma categoria
	    /// </summary>
	    /// <param name ="id">Codigo da Categoria</param>
	    /// <returns>Status 200 e objeto Categoria excluido</returns>
	    /// <remarks>Retorna status 200 e objeto Categoria excluido</remarks>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var category = await _uow.CategoryRepository.GetById(x => x.Id == id);

                if (category == null)
                {
                    return NotFound("Categoria não localizada!");
                }

                _uow.CategoryRepository.Delete(category);
                await _uow.Commit();
		
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

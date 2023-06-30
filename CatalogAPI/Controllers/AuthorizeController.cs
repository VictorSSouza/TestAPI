using CatalogAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"AuthorizeController :: Acesso em: {DateTime.Now.ToLongDateString()}";
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDTO model)
        {

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true // confirma se o email e valido
            };
            // criacao de usuario e senha
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            // efetua o login do usuario
            await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO userInfo)
        {
            // verifica as credenciais do usuario e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido");
                return BadRequest(ModelState);
            }
        }
    }
}

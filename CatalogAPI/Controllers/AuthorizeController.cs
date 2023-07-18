using CatalogAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"AuthorizeController :: Acesso em: {DateTime.Now.ToLongDateString()}";
        }

	    /// <summary>
	    /// Registra um novo usuário
	    /// </summary>
	    /// <param name ="model">Objeto do tipo UserDTO</param>
	    /// <returns>Status 200</returns>
	    /// <remarks>Retorna o status 200</remarks>
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

	    /// <summary>
	    /// Verifica as credenciais de um usuário
	    /// </summary>
	    /// <param name ="userInfo">Objeto do tipo UserDTO</param>
	    /// <returns>Status 200 e o token para o cliente</returns>
	    /// <remarks>Retorna o status 200 e um novo token para o cliente</remarks>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO userInfo)
        {
            // verifica as credenciais do usuario e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GenerateToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido");
                return BadRequest(ModelState);
            }
        }

        private UserToken GenerateToken(UserDTO userInfo)
        {
            // informacoes do usuario definidas
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("myPet", "Chico"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            // assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // tempo de expiracao do token
            var expiration = _configuration["TokenConfiguration:ExpireHours"];
            var expirationTime = DateTime.UtcNow.AddHours(double.Parse(expiration));

            // classe que representa e gera o token Jwt
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expirationTime,
              signingCredentials: credentials);

            return new UserToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationTime,
                Message = "Token JWT OK"
            };
        }
    }
}

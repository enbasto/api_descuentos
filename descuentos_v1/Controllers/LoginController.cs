using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WSDISCOUNT.Models;
using WSDISCOUNT.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WSDISCOUNT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginServices _UsersService;
        private readonly IConfiguration _Configuration;
        public LoginController(LoginServices UsersServices, IConfiguration Configuration)
        {
            _UsersService = UsersServices;
            _Configuration = Configuration;
        }
        [HttpPost]
        public async Task<ActionResult<Users>> Login([FromBody] Login Login )
        {
            try
            {
                var Usuario = Login.Usuario;
                var Password = Login.Password;
                var Registers = await _UsersService.GetUser(Usuario,Password);
                if (Registers is  null)
                {
                    return BadRequest(new {Mesagge="Datos De Logeo Incorrectos"});
                }
                var JWT = _Configuration.GetSection("JWT").Get<JWT>();
                var Claim = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,JWT.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                    new Claim("Id",Registers.Id),
                    new Claim("Usuario",Registers.Usuario),
                };
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.SecretKey));
                var SinGing = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                var Token = new JwtSecurityToken(
                    JWT.Issuer,
                    JWT.Audience,
                    Claim,
                    expires: DateTime.Now.AddMinutes(JWT.Time),
                    signingCredentials:SinGing
                   );
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(Token) });
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio una Exepcion Inesperada : " + e.Message);
            }
        }
    }
}

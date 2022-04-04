using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SCVC.Helper;
using SCVC.Models;
using SCVC.Models.ViewModel;


namespace SCVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        private readonly IConfiguration configuration;
        public LoginController(ConexionContext DbConexion, IConfiguration configuration)
        {
            this.DbConexion = DbConexion;
            this.configuration = configuration;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(LoginVM login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                var usuario = await this.DbConexion.Usuarios.Where(x => x.Usuario == login.Usuario).FirstOrDefaultAsync();
                if(usuario == null)
                {
                    return NotFound(ErrorHelper.Response(404, "Usuario No Encontrado"));
                }
                else
                {
                    if(HashHelper.CheckHash(login.Clave, usuario.PassUser, usuario.Sal))
                    {
                        var secretKey = configuration.GetValue<string>("SecretKey");
                        var key = Encoding.ASCII.GetBytes(secretKey);

                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.Usuario));

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = claims,
                            Expires = DateTime.UtcNow.AddHours(10),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                        string TokenAcceso = tokenHandler.WriteToken(createdToken);
                        return Ok(TokenAcceso);
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }
        }
    }
}
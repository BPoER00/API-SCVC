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
        public Reply Post(LoginVM login)
        {
            var oR = new Reply();
            oR.result = 0;

            try
            {
                if (!ModelState.IsValid)
                {
                    oR.message = "400 Modelo Invalido";
                }
                else
                {
                    var usuario = this.DbConexion.Usuarios.Where(x => x.Usuario == login.Usuario).FirstOrDefault();
                    if (usuario == null)
                    {
                        oR.message = "404 Usuario No Encontrado";
                    }
                    else
                    {
                        if (HashHelper.CheckHash(login.Clave, usuario.PassUser, usuario.Sal))
                        {
                            oR.result = 1;
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
                            oR.data = TokenAcceso;
                        }
                        else
                        {
                            oR.message = "Error Al Generar El Token";
                        }
                    }
                }
            }
            catch
            {
                oR.message = "Error";
            }
            return oR;
        }
    }
}
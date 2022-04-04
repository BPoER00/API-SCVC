using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCVC.Helper;
using SCVC.Models;

namespace SCVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private ConexionContext DbConexion;
        
        public UsuariosController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            List<Usuarios> Usuarios = await this.DbConexion.Usuarios.Select(x => new Usuarios(){
                IdUsuario = x.IdUsuario,
                Usuario = x.Usuario
            }).ToListAsync();

            return Ok(Usuarios);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var Usuarios = await this.DbConexion.Usuarios.Where(x => x.IdUsuario == Id).Select(x => new Usuarios(){
                IdUsuario = x.IdUsuario,
                Usuario = x.Usuario
            }).SingleOrDefaultAsync();

            return Ok(Usuarios);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Usuarios Usuario)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Usuarios.Where(x => x.Usuario == Usuario.Usuario).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "El Usuario Ingresado Ya Existe"));
                }
                else
                {
                    HashedPassword Password = HashHelper.Hash(Usuario.PassUser);
                    Usuario.PassUser = Password.Password;
                    Usuario.Estatus = 1;
                    Usuario.Sal = Password.Salt;

                    this.DbConexion.Usuarios.Add(Usuario);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
                }
            }
        }
    }
}
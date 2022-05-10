using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCVC.Helper;
using SCVC.Models;

namespace SCVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Usuarios usuario)
        {
            if(usuario.IdUsuario == 0)
            {
                usuario.IdUsuario = id;
            }
            else if(usuario.IdUsuario != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Usuarios.Where(p => p.IdUsuario == id).AsTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Usuario No Encontrado"));
            }
            else
            {
                HashedPassword Password = HashHelper.Hash(usuario.PassUser);
                usuario.PassUser = Password.Password;
                usuario.Estatus = 1;
                usuario.Sal = Password.Salt;                
    
                this.DbConexion.Entry(usuario).State = EntityState.Modified;

                if(!ModelState.IsValid)
                {
                    return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
                }
                else
                {
                    await this.DbConexion.SaveChangesAsync();
                    return NoContent();
                }
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var Usuarios = await this.DbConexion.Usuarios.FindAsync(id);
            if(Usuarios == null)
            {
                return BadRequest(ErrorHelper.Response(404, "Usuario No Encontrado"));
            }
            else
            {
                this.DbConexion.Usuarios.Remove(Usuarios);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
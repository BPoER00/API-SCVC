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
    public class RolesController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public RolesController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var Rol = await this.DbConexion.Roles.ToListAsync();

            return Ok(Rol);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Rol = await this.DbConexion.Roles.FindAsync(id);
            if(Rol == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Rol);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Roles Rol)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Roles.Where(r => r.NombreRol == Rol.NombreRol).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Este Dato Ya Existe"));
                }
                else
                {
                    this.DbConexion.Add(Rol);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Roles Rol)
        {
            if(Rol.IdRol == 0)
            {
                Rol.IdRol = id;
            }
            else if(Rol.IdRol != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Roles.Where(r => r.IdRol == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(Rol).State = EntityState.Modified;

                if(!TryValidateModel(Rol, nameof(Rol)))
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
            var Rol = await this.DbConexion.Roles.FindAsync(id);
            if(Rol == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(Rol);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }          
    }
}
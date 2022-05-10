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
    public class RolUsuarioController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public RolUsuarioController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var RolU = await this.DbConexion.RolU.ToListAsync();

            return Ok(RolU);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var RolU = await this.DbConexion.RolU.FindAsync(id);
            if(RolU == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(RolU);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(RolUsuario RolU)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.RolU.Where(r => r.NombreRol == RolU.NombreRol).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.RolU.Add(RolU);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, RolUsuario RolU)
        {
            if(RolU.IdRol == 0)
            {
                RolU.IdRol = id;
            }
            else if(RolU.IdRol != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.RolU.Where(r => r.IdRol == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(RolU.IdRol).State = EntityState.Modified;

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
            var RolU = await this.DbConexion.RolU.FindAsync(id);
            if(RolU == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.RolU.Remove(RolU);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                
    }
}
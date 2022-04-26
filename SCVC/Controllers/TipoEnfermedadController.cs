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
    public class TipoEnfermedadController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public TipoEnfermedadController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var tipo = await this.DbConexion.TipoEnfermedad.ToListAsync();

            return Ok(tipo);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var tipo = await this.DbConexion.TipoEnfermedad.FindAsync(id);
            if(tipo == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(tipo);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(TipoEnfermedad tipoEnfermedad)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.TipoEnfermedad.Where(t => t.NombreEnfermedad == tipoEnfermedad.NombreEnfermedad).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Add(tipoEnfermedad);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, TipoEnfermedad tipoEnfermedad)
        {
            if(tipoEnfermedad.IdTipoEnfermedad == 0)
            {
                tipoEnfermedad.IdTipoEnfermedad = id;
            }
            else if(tipoEnfermedad.IdTipoEnfermedad != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.TipoEnfermedad.Where(t => t.IdTipoEnfermedad == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(tipoEnfermedad).State = EntityState.Modified;

                if(!TryValidateModel(tipoEnfermedad, nameof(tipoEnfermedad)))
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
            var tipoEnfermedad = await this.DbConexion.TipoEnfermedad.FindAsync(id);
            if(tipoEnfermedad == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(tipoEnfermedad);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }        
    }
}
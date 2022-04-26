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
    public class ClasificacionController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public ClasificacionController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var Clasificaciones = await this.DbConexion.Clasificacion.ToListAsync();

            return Ok(Clasificaciones);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Clasificaciones = await this.DbConexion.Clasificacion.FindAsync(id);
            if(Clasificaciones == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Clasificaciones);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Clasificacion clasificacion)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Clasificacion.Where(x => x.NombreClasificacion == clasificacion.NombreClasificacion).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Add(clasificacion);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Clasificacion clasificacion)
        {
            if(clasificacion.IdClasificacion == 0)
            {
                clasificacion.IdClasificacion = id;
            }
            else if(clasificacion.IdClasificacion != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Clasificacion.Where(c => c.IdClasificacion == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(clasificacion).State = EntityState.Modified;

                if(!TryValidateModel(clasificacion, nameof(clasificacion)))
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
            var clasificacion = await this.DbConexion.Clasificacion.FindAsync(id);
            if(clasificacion == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(clasificacion);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
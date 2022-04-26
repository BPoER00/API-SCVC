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
    public class GeneracionesController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public GeneracionesController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var generaciones = await this.DbConexion.Generaciones.ToListAsync();

            return Ok(generaciones);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var generaciones = await this.DbConexion.Generaciones.FindAsync(id);
            if(generaciones == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(generaciones);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Generaciones generaciones)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Generaciones.Where(g => g.Generacion == generaciones.Generacion).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Add(generaciones);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Generaciones generaciones)
        {
            if(generaciones.IdGeneracion == 0)
            {
                generaciones.IdGeneracion = id;
            }
            else if(generaciones.IdGeneracion != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Generaciones.Where(g => g.IdGeneracion == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(generaciones).State = EntityState.Modified;

                if(!TryValidateModel(generaciones, nameof(generaciones)))
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
            var generaciones = await this.DbConexion.Generaciones.FindAsync(id);
            if(generaciones == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(generaciones);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }        
    }
}
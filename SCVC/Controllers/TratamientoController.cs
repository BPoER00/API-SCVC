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
    public class TratamientoController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public TratamientoController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var tratamientos = await this.DbConexion.Tratamientos.ToListAsync();

            return Ok(tratamientos);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var tratamientos = await this.DbConexion.Tratamientos.FindAsync(id);
            if(tratamientos == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(tratamientos);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Tratamiento tratamientos)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Tratamientos.Where(t => t.NombreTratamiento == tratamientos.NombreTratamiento).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Tratamientos.Add(tratamientos);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Tratamiento tratamientos)
        {
            if(tratamientos.IdTratamiento == 0)
            {
                tratamientos.IdTratamiento = id;
            }
            else if(tratamientos.IdTratamiento != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Tratamientos.Where(t => t.IdTratamiento == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(tratamientos).State = EntityState.Modified;

                if(!TryValidateModel(tratamientos, nameof(tratamientos)))
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
            var tratamientos = await this.DbConexion.Tratamientos.FindAsync(id);
            if(tratamientos == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Tratamientos.Remove(tratamientos);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                
    }
}
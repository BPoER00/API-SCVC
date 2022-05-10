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
    public class TipoConsultaController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public TipoConsultaController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var tipo = await this.DbConexion.TipoConsultas.ToListAsync();

            return Ok(tipo);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var tipo = await this.DbConexion.TipoConsultas.FindAsync(id);
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
        public async Task<IActionResult> Post(TipoConsulta tipoConsulta)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                this.DbConexion.TipoConsultas.Add(tipoConsulta);
                await this.DbConexion.SaveChangesAsync();
                return Ok();           
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, TipoConsulta tipoConsulta)
        {
            if(tipoConsulta.IdTipoContulta == 0)
            {
                tipoConsulta.IdTipoContulta = id;
            }
            else if(tipoConsulta.IdTipoContulta != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.TipoConsultas.Where(t => t.IdTipoContulta == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(tipoConsulta).State = EntityState.Modified;

                if(!TryValidateModel(tipoConsulta, nameof(tipoConsulta)))
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
            var tipo = await this.DbConexion.TipoConsultas.FindAsync(id);
            if(tipo == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.TipoConsultas.Remove(tipo);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                              
    }
} 
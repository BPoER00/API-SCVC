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
    public class MotivoConsultaController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public MotivoConsultaController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var motivo = await this.DbConexion.MotivoConsultas.ToListAsync();

            return Ok(motivo);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var motivo = await this.DbConexion.MotivoConsultas.FindAsync(id);
            if(motivo == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(motivo);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(MotivoConsulta motivoConsulta)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                this.DbConexion.MotivoConsultas.Add(motivoConsulta);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, MotivoConsulta motivoConsulta)
        {
            if(motivoConsulta.IdMotivoConsulta == 0)
            {
                motivoConsulta.IdMotivoConsulta = id;
            }
            else if(motivoConsulta.IdMotivoConsulta != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.MotivoConsultas.Where(m => m.IdMotivoConsulta == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(motivoConsulta).State = EntityState.Modified;

                if(!TryValidateModel(motivoConsulta, nameof(motivoConsulta)))
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
            var motivoConsulta = await this.DbConexion.MotivoConsultas.FindAsync(id);
            if(motivoConsulta == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.MotivoConsultas.Remove(motivoConsulta);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                
    }
}
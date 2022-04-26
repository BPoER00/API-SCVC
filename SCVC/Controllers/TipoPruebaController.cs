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
    public class TipoPruebaController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public TipoPruebaController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var tipo = await this.DbConexion.TipoPruebas.ToListAsync();

            return Ok(tipo);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var tipo = await this.DbConexion.TipoPruebas.FindAsync(id);
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
        public async Task<IActionResult> Post(TipoPrueba tipoPrueba)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.TipoPruebas.Where(t => t.NombrePrueba == tipoPrueba.NombrePrueba).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Add(tipoPrueba);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, TipoPrueba tipoPrueba)
        {
            if(tipoPrueba.IdTipoPrueba == 0)
            {
                tipoPrueba.IdTipoPrueba = id;
            }
            else if(tipoPrueba.IdTipoPrueba != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.TipoPruebas.Where(t => t.IdTipoPrueba == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(tipoPrueba).State = EntityState.Modified;

                if(!TryValidateModel(tipoPrueba, nameof(tipoPrueba)))
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
            var tipoPrueba = await this.DbConexion.TipoPruebas.FindAsync(id);
            if(tipoPrueba == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(tipoPrueba);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                
    }
}
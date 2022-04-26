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
    public class VigilanciaController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public VigilanciaController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var vigilancias = await this.DbConexion.Vigilancias.ToListAsync();

            return Ok(vigilancias);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var vigilancias = await this.DbConexion.Vigilancias.FindAsync(id);
            if(vigilancias == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(vigilancias);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Vigilancia vigilancias)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Vigilancias.Where(v => v.NombreVigilancia == vigilancias.NombreVigilancia).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Add(vigilancias);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Vigilancia vigilancias)
        {
            if(vigilancias.IdVigilancia == 0)
            {
                vigilancias.IdVigilancia = id;
            }
            else if(vigilancias.IdVigilancia != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Vigilancias.Where(v => v.IdVigilancia == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(vigilancias).State = EntityState.Modified;

                if(!TryValidateModel(vigilancias, nameof(vigilancias)))
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
            var vigilancias = await this.DbConexion.Vigilancias.FindAsync(id);
            if(vigilancias == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(vigilancias);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }                
    }
}
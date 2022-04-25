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
    public class EtniasController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public EtniasController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var etnias = await this.DbConexion.Etnias.ToListAsync();

            return Ok(etnias);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Etnias = await this.DbConexion.Etnias.FindAsync(id);
            if(Etnias == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Etnias);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Etnias etnias)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Etnias.Where(x => x.NombreEtnias == etnias.NombreEtnias).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Este Dato Ya Existe"));
                }
                else
                {
                    this.DbConexion.Add(etnias);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Etnias etinas)
        {
            if(etinas.IdEtnia == 0)
            {
                etinas.IdEtnia = id;
            }
            else if(etinas.IdEtnia != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Etnias.Where(e => e.IdEtnia == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(etinas).State = EntityState.Modified;

                if(!TryValidateModel(etinas, nameof(etinas)))
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
            var Etnias = await this.DbConexion.Etnias.FindAsync(id);
            if(Etnias == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Remove(Etnias);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
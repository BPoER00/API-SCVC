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
    public class GenerosController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public GenerosController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var generos = await this.DbConexion.Generos.ToListAsync();

            return Ok(generos);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Generos = await this.DbConexion.Generos.FindAsync(id);
            if(Generos == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Generos);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Generos generos)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Generos.Where(x => x.NombreGenero == generos.NombreGenero).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Generos.Add(generos);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Generos generos)
        {
            if(generos.IdGenero == 0)
            {
                generos.IdGenero = id;
            }
            else if(generos.IdGenero != id)
            {
                return BadRequest(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            if(!await this.DbConexion.Generos.Where(g => g.IdGenero == id).AsNoTracking().AnyAsync())
            {
                return NotFound();
            }
            else
            {
                this.DbConexion.Entry(generos).State = EntityState.Modified;

                if(!TryValidateModel(generos, nameof(generos)))
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
            var Generos = await this.DbConexion.Generos.FindAsync(id);
            if(Generos == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Generos.Remove(Generos);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
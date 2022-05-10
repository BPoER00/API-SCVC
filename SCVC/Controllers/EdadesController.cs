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
    public class EdadesController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public EdadesController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var edades = await this.DbConexion.Edades.ToListAsync();

            return Ok(edades);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Edades = await this.DbConexion.Edades.FindAsync(id);
            if(Edades == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Edades);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Edades edades)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Edades.Where(x => x.NombreEdad == edades.NombreEdad).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Este Dato Ya Existe"));
                }
                else
                {
                    this.DbConexion.Edades.Add(edades);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Edades edades)
        {
            if(edades.IdEdad == 0)
            {
                edades.IdEdad = id;
            }
            else if(edades.IdEdad != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Edades.Where(e => e.IdEdad == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(edades).State = EntityState.Modified;

                if(!TryValidateModel(edades, nameof(edades)))
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
            var Edadades = await this.DbConexion.Edades.FindAsync(id);
            if(Edadades == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Edades.Remove(Edadades);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }  
    }
}
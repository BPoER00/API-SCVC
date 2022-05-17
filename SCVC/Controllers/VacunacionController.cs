using System.Collections.Generic;
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
    public class VacunacionController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public VacunacionController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var vacunacion = await this.DbConexion.Vacunacion.Include(x => x.Vacunas)
            .Include(x => x.Persona).Include(x => x.Usuarios).ToListAsync();

            return Ok(vacunacion);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var vacunacion = await this.DbConexion.Vacunacion.Where(x => x.IdVacunacion == id).Include(x => x.Vacunas)
            .Include(x => x.Persona).Include(x => x.Usuarios).SingleOrDefaultAsync();
            if(vacunacion == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(vacunacion);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Vacunacion vacunacion)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Vacunacion.Where(x => x.IdVacunacion == vacunacion.IdVacunacion).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Vacunacion.Add(vacunacion);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Vacunacion vacunacion)
        {
            if(vacunacion.IdVacunacion == 0)
            {
                vacunacion.IdVacunacion = id;
            }
            else if(vacunacion.IdVacunacion != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Vacunacion.Where(x => x.IdVacunacion == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(vacunacion).State = EntityState.Modified;

                if(!ModelState.IsValid)
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
            var vacunacion = await this.DbConexion.Vacunacion.FindAsync(id);
            if(vacunacion == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Vacunacion.Remove(vacunacion);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
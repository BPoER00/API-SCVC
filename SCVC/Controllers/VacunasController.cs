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
    public class VacunasController : ControllerBase
    {
        private readonly ConexionContext DbConexion;
        
        public VacunasController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var vacunas = await this.DbConexion.Vacunas.Include(x => x.Generaciones).ToListAsync();

            return Ok(vacunas);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var vacunas = await this.DbConexion.Vacunas
            .Where(x => x.IdVacuna == id).Include(x => x.Generaciones).SingleOrDefaultAsync();
            if(vacunas == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(vacunas);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Vacunas vacunas)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Vacunas.Where(x => x.IdVacuna == vacunas.IdVacuna).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Vacunas.Add(vacunas);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Vacunas vacunas)
        {
            if(vacunas.IdVacuna == 0)
            {
                vacunas.IdVacuna = id;
            }
            else if(vacunas.IdVacuna != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Vacunas.Where(x => x.IdVacuna == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(vacunas).State = EntityState.Modified;

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
            var vacunas = await this.DbConexion.Vacunas.FindAsync(id);
            if(vacunas == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Vacunas.Remove(vacunas);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
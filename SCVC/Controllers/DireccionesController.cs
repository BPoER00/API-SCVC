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
    public class DireccionesController : ControllerBase
    {

        private readonly ConexionContext DbConexion;
        
        public DireccionesController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var direcciones = await this.DbConexion.Direcciones.ToListAsync();

            return Ok(direcciones);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Direccion = await this.DbConexion.Direcciones.FindAsync(id);
            if(Direccion == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                return Ok(Direccion);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(Direcciones direcciones)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.Direcciones.Where(x => x.NombreDireccion == direcciones.NombreDireccion).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Dato Ya Existente"));
                }
                else
                {
                    this.DbConexion.Direcciones.Add(direcciones);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, Direcciones Direccion)
        {
            if(Direccion.IdDireccion == 0)
            {
                Direccion.IdDireccion = id;
            }
            else if(Direccion.IdDireccion != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.Direcciones.Where(d => d.IdDireccion == id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(Direccion).State = EntityState.Modified;

                if(!TryValidateModel(Direccion, nameof(Direccion)))
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
            var Direccion = await this.DbConexion.Direcciones.FindAsync(id);
            if(Direccion == null)
            {
                return NotFound(ErrorHelper.Response(404, "Dato No Encontrado"));
            }
            else
            {
                this.DbConexion.Direcciones.Remove(Direccion);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
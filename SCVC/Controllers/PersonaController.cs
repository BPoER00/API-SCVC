using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCVC.Helper;
using SCVC.Models;
using SCVC.Models.ViewModel;

namespace SCVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PersonaController : ControllerBase
    {

        private readonly ConexionContext DbConexion;

        public PersonaController(ConexionContext DbConexion)
        {
            this.DbConexion = DbConexion;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var personas = await this.DbConexion.TBL_Personas.Include(x => x.Direcciones).Include(x => x.Generos)
            .Include(x => x.Etnias).Include(x => x.Edades).Include(x => x.Roles).ToListAsync();

            return Ok(personas);
        }

        [HttpGet("GetEnfermero")]
        public async Task<IActionResult> GetEnfermero()
        {
            var personas = await this.DbConexion.TBL_Personas.Where(x => x.IdRol == 2).Include(x => x.Direcciones).Include(x => x.Generos)
            .Include(x => x.Etnias).Include(x => x.Edades).Include(x => x.Roles).ToListAsync();

            return Ok(personas);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var Persona = await this.DbConexion.TBL_Personas.Where(x => x.IdPersona == id).Include(x => x.Direcciones).Include(x => x.Generos)
            .Include(x => x.Etnias).Include(x => x.Edades).Include(x => x.Roles).SingleOrDefaultAsync();
            
            if(Persona == null)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario No Encontrado"));
            }
            else
            {
                return Ok(Persona);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(TBL_Persona persona)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else
            {
                if(await this.DbConexion.TBL_Personas.Where(x => x.CUI == persona.CUI).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, "Este CUI Ya Existe"));
                }
                else
                {
                    this.DbConexion.TBL_Personas.Add(persona);
                    await this.DbConexion.SaveChangesAsync();
                    return Ok();
               }
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, TBL_Persona persona)
        {
            if(persona.IdPersona == 0)
            {
                persona.IdPersona = id;
            }
            else if(persona.IdPersona != id)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            if(!await this.DbConexion.TBL_Personas.Where(p => p.IdPersona == id).AsTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "Usuario No Encontrado"));
            }
            else
            {
                this.DbConexion.Entry(persona).State = EntityState.Modified;

                if(!TryValidateModel(persona, nameof(persona)))
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
            var Persona = await this.DbConexion.TBL_Personas.FindAsync(id);
            if(Persona == null)
            {
                return BadRequest(ErrorHelper.Response(404, "Usuario No Encontrado"));
            }
            else
            {
                this.DbConexion.TBL_Personas.Remove(Persona);
                await this.DbConexion.SaveChangesAsync();
                return Ok();
            }
        }
    }

}
using Microsoft.EntityFrameworkCore;

namespace SCVC.Models
{
    public class ConexionContext : DbContext
    {
        public ConexionContext(DbContextOptions<ConexionContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<RolUsuario> RolU { get; set; } 
        public DbSet<Direcciones> Direcciones { get; set; }
        public DbSet<Etnias> Etnias { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Edades> Edades { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TBL_Persona> TBL_Personas { get; set; }
        public DbSet<Clasificacion> Clasificacion { get; set; }
        public DbSet<Generaciones> Generaciones { get; set; }
        public DbSet<MotivoConsulta> MotivoConsultas { get; set; }
        public DbSet<TipoConsulta> TipoConsultas { get; set; }
        public DbSet<TipoEnfermedad> TipoEnfermedad { get; set; }
        public DbSet<TipoPrueba> TipoPruebas { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Vigilancia> Vigilancias { get; set; } 
    }
}
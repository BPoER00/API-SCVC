using Microsoft.EntityFrameworkCore;

namespace SCVC.Models
{
    public class ConexionContext : DbContext
    {
        public ConexionContext(DbContextOptions<ConexionContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Direcciones> Direcciones { get; set; }
        public DbSet<Etnias> Etnias { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Edades> Edades { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TBL_Persona> TBL_Personas { get; set; }


    }
}
using Microsoft.EntityFrameworkCore;

namespace SCVC.Models
{
    public class ConexionContext : DbContext
    {
        public ConexionContext(DbContextOptions<ConexionContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
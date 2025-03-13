using API_Technical_Test.Modelos;
using Microsoft.EntityFrameworkCore;

namespace API_Technical_Test.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Empleados> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>().HasData(
                new Clientes()
                {
                    Id = 1,
                    Nombre = "Villa Langostura",
                    Detalle = "450 kilometros",
                    Cuenta = 5101,
                    FechaCreacion = DateTime.Now
                },
                new Clientes()
                {
                    Id = 2,
                    Nombre = "Villa Mercedes",
                    Detalle = "320 kilometros",
                    Cuenta = 4223,
                    FechaCreacion = DateTime.Now
                }
                );
        }
    }
}

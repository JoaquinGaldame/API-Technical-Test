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

    }
}

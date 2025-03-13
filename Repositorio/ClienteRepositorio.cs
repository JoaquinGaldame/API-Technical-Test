using API_Technical_Test.Datos;
using API_Technical_Test.Modelos;
using API_Technical_Test.Repositorio.IRepositorio;

namespace API_Technical_Test.Repositorio
{
    public class ClienteRepositorio : Repositorio<Clientes>, IClienteRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ClienteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Clientes> Actualizar(Clientes entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Clientes.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}

using API_Technical_Test.Modelos;

namespace API_Technical_Test.Repositorio.IRepositorio
{
    public interface IClienteRepositorio : IRepositorio<Clientes>
    {
        Task<Clientes> Actualizar(Clientes entidad);
    }
}

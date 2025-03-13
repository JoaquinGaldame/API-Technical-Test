using API_Technical_Test.Modelos;
using API_Technical_Test.Modelos.DTO;
using API_Technical_Test.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace API_Technical_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        // Inyectamos el servicio de Logger para mostrar info por consola
        private readonly ILogger<ClienteController> _logger;
        // Variable privada para la Conexión de la Base de Datos
        // private readonly ApplicationDbContext _db; YA NO LO USAMOS
        private readonly IClienteRepositorio _clienteRepo;
        // Agregamos Mapper
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public ClienteController(ILogger<ClienteController> logger, IClienteRepositorio clienteRepo, IMapper mapper)
        {
            _logger = logger;
            _clienteRepo = clienteRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener Lista de Clientes");

                //Creamos una lista de tipo Cliente extraida de la DB
                //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
                IEnumerable<Clientes> clientesLista = await _clienteRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<ClienteDTO>>(clientesLista);
                _response.statusCode = HttpStatusCode.OK;
                //Retorna un IEnumerable del tipo ClienteDTO obteniendo los datos de clientesLista
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}

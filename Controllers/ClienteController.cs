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
        public async Task<ActionResult<APIResponse>> GetCustomers()
        {
            try
            {
                _logger.LogInformation("Obtener Lista de Clientes");

                //Creamos una lista de tipo Cliente extraida de la DB
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

        [HttpGet("{id:int}", Name = "GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCustomerByID(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Cliente con Id " + id);
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //Enviamos la Expresion LINQ entre ()
                var customer = await _clienteRepo.Obtener(v => v.Id == id);

                if (customer == null)
                {
                    _logger.LogError("Error No existe Cliente con Id " + id);
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _logger.LogInformation("Obtener Cliente con ID " + id);

                //Solo aplicamos el AutoMapper
                //Retornamos un Objeto del tipo VillaDto
                //Obtenemos los datos de la variable villa 
                _response.Resultado = _mapper.Map<ClienteDTO>(customer);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateCustomer([FromBody] ClienteCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Error al crear nuevo Cliente - modelo no valido");
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages?.Add("Error al crear nuevo Cliente - Datos no válidos");
                    return BadRequest(_response);
                }

                if (await _clienteRepo.Obtener(v => v.DNI.ToLower() == createDto.dni.ToLower()) != null)
                {
                    _logger.LogError("Error al crear nuevo Cliente - Cliente existente");
                    ModelState.AddModelError("Nombre Existe", "Cliente con ese Nombre ya existe");
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages?.Add("Error al crear nuevo Cliente - Cliente existente");
                    return BadRequest(_response);
                }

                if (createDto == null)
                {
                    _logger.LogError("Error al enviar modelo de Cliente");
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages?.Add("Error al crear Cliente");
                    return BadRequest(_response);
                }


                Clientes modelo = _mapper.Map<Clientes>(createDto);

                // await _db.Villas.AddAsync(modelo);
                // await _db.SaveChangesAsync(); NO lo usamos porque viene incorporado en el IRepository
                modelo.FechaActualizacion = DateTime.Now;
                modelo.FechaCreacion = DateTime.Now;
                await _clienteRepo.Crear(modelo);

                //Para indicar la URL del recurso creado
                _logger.LogInformation("Creación de Cliente con Éxito");
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCustomerByID", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                //El método Delete no necesita Mapeo
                if (id == 0)
                {
                    _logger.LogError("Error al traer Cliente con Id " + id);
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var cliente = await _clienteRepo.Obtener(v => v.Id == id);

                if (cliente == null)
                {
                    _logger.LogError("Error No existe Cliente con Id " + id);
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                //Eliminamos registro en la DB - No existe Remove asíncrono
                await _clienteRepo.Remover(cliente);
                _logger.LogInformation("Eliminación de Cliente ID " + id);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] ClienteUpdateDTO updateDto)
        {
            if ((updateDto == null || id != updateDto.Id))
            {
                _logger.LogError("Error al traer Cliente con Id " + id);
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Clientes modelo = _mapper.Map<Clientes>(updateDto);

            await _clienteRepo.Actualizar(modelo);
            var clienteActualizado = await _clienteRepo.Obtener(c => c.Id == id);

            if (clienteActualizado == null)
            {
                _logger.LogError("Error al obtener el cliente actualizado");
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _logger.LogInformation("Actualización PUT de Cliente");
            _response.IsExitoso = true;
            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = clienteActualizado; // Devuelve el cliente actualizado
            return Ok(_response);

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<ClienteUpdateDTO> patchDto)
        {
            if ((patchDto == null || id == 0))
            {
                _logger.LogError("Error al traer Villa con Id " + id);
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var cliente = await _clienteRepo.Obtener(v => v.Id == id, tracked: false);

            ClienteUpdateDTO clienteDto = _mapper.Map<ClienteUpdateDTO>(cliente);

            if (cliente == null)
            {
                _logger.LogError("Error Cliente Inexistente");
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            patchDto.ApplyTo(clienteDto, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Error Modelo de Cliente no valido");
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Clientes modelo = _mapper.Map<Clientes>(clienteDto);

            await _clienteRepo.Actualizar(modelo);

            var clienteActualizado = await _clienteRepo.Obtener(c => c.Id == id);

            if (clienteActualizado == null)
            {
                _logger.LogError("Error al obtener el cliente actualizado");
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _logger.LogInformation("Actualización Cliente de Villa");
            _response.IsExitoso = true;
            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = clienteActualizado;
            return Ok(_response);
        }
    }
}

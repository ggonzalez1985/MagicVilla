using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Modelos.Especificaciones;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Net;

namespace MagicVilla_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaController : ControllerBase
    {
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaController(IVillaRepositorio villaRepo, IMapper mapper)
        {
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }

        //Con métodos asíncronos, el servidor puede gestionar las 100 solicitudes al mismo tiempo, ya que mientras espera las respuestas de la base de datos, sigue atendiendo nuevas solicitudes.
        //Esto mejora la escalabilidad porque permite manejar más usuarios sin necesitar más recursos.
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")] //llamo al perfil donde esta definido eso. 'Program.cs'
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {

            try
            {
                IEnumerable<Villa> villalist = await _villaRepo.ObtenerTodos();

                //Esto toma la lista de objetos Villa (villalist) y la convierte a una colección de VillaDto
                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villalist);
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

		[HttpGet("VillasPaginado")]
        [ResponseCache(CacheProfileName = "Default30")] //llamo al perfil donde esta definido eso. 'Program.cs'
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetVillasPaginado([FromQuery] Parametros parametros)
		{

			try
			{
                var villaList = _villaRepo.ObtenerTodosPaginado(parametros);

				//Esto toma la lista de objetos Villa (villalist) y la convierte a una colección de VillaDto
				_response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
				_response.statusCode = HttpStatusCode.OK;
                _response.TotalPaginas = villaList.MetaData.TotalPages;

				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsExitoso = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Obtener(V => V.Id == id);

                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;

                //usas el maper para devolvert un VillaDto gracias a mapper.
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "La Villa con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                    return BadRequest(createDto);

                Villa modelo = _mapper.Map<Villa>(createDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                //esto hace un INSERT  de 'modelo' con los datos posta.
                await _villaRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                //crea un objeto anónimo con una propiedad llamada id cuyo valor es el Id del objeto modelo.
                //CreatedAtRoute es que no solo indica que el recurso fue creado, sino que además devuelve la URL donde se puede acceder al recurso recién creado.
                //CreatedAtRoute: confirmación de creación
                //CreatedAtRoute es la mejor práctica cuando estás manejando la creación de un nuevo recurso en una API REST
                //Devuelve el estado correcto: 201 (Created).
                //Proporciona la URL del nuevo recurso al cliente.
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _villaRepo.Remove(villa);
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || updateDto.Id != id)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //las entidades o modelos son los que están directamente asociados a la base de datos, mientras que los DTOs son usados principalmente para transferencia de datos entre el cliente y el servidor, y no representan exactamente lo que se almacena en la base de datos.
                Villa modelo = _mapper.Map<Villa>(updateDto);

                await _villaRepo.Actualizar(modelo);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {//IActionResult y ActionResult son dos tipos comunes utilizados en ASP.NET Core para definir los resultados de las acciones de un controlador

            try
            {
                if (patchDto == null || id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

                //lo convertis en el villaDto de arriba para poder mapear los cambios.

                //controlar si la Villa no es null. Si es devuelvo BadRequest
                if (villaDto == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //aca lo pasas de updateVillaDto a Villa para guardar en la BD
                Villa modelo = _mapper.Map<Villa>(villaDto);

                //despues de esto tengo que crear una Villa para mandar con los datos a la _bd
                await _villaRepo.Actualizar(modelo);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_response);
        }
    }
}

using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly AplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaController(AplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //Con métodos asíncronos, el servidor puede gestionar las 100 solicitudes al mismo tiempo, ya que mientras espera las respuestas de la base de datos, sigue atendiendo nuevas solicitudes.
        //Esto mejora la escalabilidad porque permite manejar más usuarios sin necesitar más recursos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            IEnumerable<Villa> villalist = await _db.Villas.ToListAsync();

            //Esto toma la lista de objetos Villa (villalist) y la convierte a una colección de VillaDto
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villalist));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {

            if (id == 0)
                return BadRequest();

            var villa = await _db.Villas.FirstOrDefaultAsync(V => V.Id == id);

            if (villa == null)
                return NotFound();

            //usas el maper para devolvert un VillaDto gracias a mapper.
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (createDto == null)
                return BadRequest(createDto);

            Villa modelo = _mapper.Map<Villa>(createDto);

            //esto hace un INSERT  de 'modelo' con los datos posta.
            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();

            //crea un objeto anónimo con una propiedad llamada id cuyo valor es el Id del objeto modelo.
            //CreatedAtRoute es que no solo indica que el recurso fue creado, sino que además devuelve la URL donde se puede acceder al recurso recién creado.
            //CreatedAtRoute: confirmación de creación
            //CreatedAtRoute es la mejor práctica cuando estás manejando la creación de un nuevo recurso en una API REST
            //Devuelve el estado correcto: 201 (Created).
            //Proporciona la URL del nuevo recurso al cliente.
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
                return NotFound();

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || updateDto.Id != id)
                return BadRequest();

            //las entidades o modelos son los que están directamente asociados a la base de datos, mientras que los DTOs son usados principalmente para transferencia de datos entre el cliente y el servidor, y no representan exactamente lo que se almacena en la base de datos.
            Villa modelo = _mapper.Map<Villa>(updateDto);

            _db.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {//IActionResult y ActionResult son dos tipos comunes utilizados en ASP.NET Core para definir los resultados de las acciones de un controlador
            if (patchDto == null || id == 0)
                return BadRequest();

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            //lo convertis en el villaDto de arriba para poder mapear los cambios.

            //controlar si la Villa no es null. Si es devuelvo BadRequest
            if (villaDto == null)
                return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //aca lo pasas de updateVillaDto a Villa para guardar en la BD
            Villa modelo = _mapper.Map<Villa>(villaDto);

            //despues de esto tengo que crear una Villa para mandar con los datos a la _bd
            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}

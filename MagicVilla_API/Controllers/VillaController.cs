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
        public VillaController(AplicationDbContext db)
        {
                _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() 
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id) 
        {

            if (id == 0)
                return BadRequest();

            var villa = _db.Villas.FirstOrDefault(V => V.Id == id);
            //VillaStore.villaList.FirstOrDefault(V => V.Id == id);

            if (villa == null)
                return NotFound();

          return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaCreateDto villaDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            { 
                ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
                return BadRequest();
            }

            if (villaDto == null)
                return BadRequest();

            Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };

            //esto hace un INSERT  de 'modelo' con los datos posta.
            _db.Villas.Add(modelo);
            _db.SaveChanges();

            //crea un objeto anónimo con una propiedad llamada id cuyo valor es el Id del objeto modelo.
            //CreatedAtRoute es que no solo indica que el recurso fue creado, sino que además devuelve la URL donde se puede acceder al recurso recién creado.
            //CreatedAtRoute: confirmación de creación
            //CreatedAtRoute es la mejor práctica cuando estás manejando la creación de un nuevo recurso en una API REST
            //Devuelve el estado correcto: 201 (Created).
            //Proporciona la URL del nuevo recurso al cliente.
            return CreatedAtRoute("GetVilla", new {id= modelo.Id}, modelo);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0) 
                return BadRequest();

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
                return NotFound();

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();    

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {
            if(villaDto == null || villaDto.Id != id)
                return BadRequest();

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad,
                FechaActualizacion = DateTime.Now
            };

            _db.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
                return BadRequest();

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle, 
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl= villa.ImagenUrl,
                Amenidad= villa.Amenidad
            };

            //controlar si la Villa no es null. Si es devuelvo BadRequest
            if(villaDto == null)
                return BadRequest(); 

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //despues de esto tengo que crear una Villa para mandar con los datos a la _bd
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre= villaDto.Nombre,
                Detalle= villaDto.Detalle,
                Tarifa= villaDto.Tarifa,
                Ocupantes= villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

    }
}

using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService; //propiedades para hacer inyeccion de dependencia.
        private readonly IMapper _mapper; 

        public VillaController(IVillaService villaServices, IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaServices; //de esta manera ya estan inyectados los servicios y los podemos utilizar.
        }

        public async Task<IActionResult> IndexVilla() //cada metodo que vamos a crear esta relacionado con una vista.
        {
            List<VillaDto> villaList = new();

            var response = await _villaService.ObtenerTodos<APIResponse>(); //APIResponse es la clase que nos retorna los elementos                                                                 de APIResponse 

            if (response != null && response.IsExitoso)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(villaList);
        }
    }
}

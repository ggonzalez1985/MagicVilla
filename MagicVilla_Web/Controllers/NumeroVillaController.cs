using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
	public class NumeroVillaController : Controller
	{
		private readonly INumeroVillaService _numeroVillaService;
		private readonly IMapper _mapper;

        public NumeroVillaController(INumeroVillaService numeroVillaService, IMapper mapper)
        {
			_numeroVillaService = numeroVillaService;
            _mapper = mapper;
        }

		public async Task<IActionResult> IndexNumeroVilla()
		{
			List<NumeroVillaDto> numeroVillaList = new();

			var response = await _numeroVillaService.ObtenerTodos<APIResponse>();

			if (response != null && response.IsExitoso)
			{
				numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDto>>(Convert.ToString(response.Resultado));
			}

			return View(numeroVillaList);
		}

		public async Task<IActionResult> CrearNumeroVilla()
		{
			return View();
		}

		//crea 1 ViewModel para pasarle los datos de las villas disponibles.
		//ViewModel encapsula varios modelos en 1 sola clase.


       
	}
}

using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
	public class NumeroVillaController : Controller
	{
		private readonly INumeroVillaService _numeroVillaService;
		private readonly IVillaService _villaService;
		private readonly IMapper _mapper;

		public NumeroVillaController(INumeroVillaService numeroVillaService, IMapper mapper, IVillaService villaService)
		{
			_villaService = villaService;
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
			NumeroVillaViewModel numeroVillaVM = new();

			var response = await _villaService.ObtenerTodos<APIResponse>();

			if (response != null && response.IsExitoso)
			{
				numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
											.Select(v => new SelectListItem
											{
												Text = v.Nombre,
												Value = v.Id.ToString(),
											}); //de esta forma llenas la lista de villa lista
			}

			return View(numeroVillaVM);
		}

		//crea 1 ViewModel para pasarle los datos de las villas disponibles.
		//ViewModel encapsula varios modelos en 1 sola clase.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CrearNumeroVilla(NumeroVillaViewModel modelo)
		{
			if (ModelState.IsValid)
			{
				var response = await _numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla);
				if (response != null && response.IsExitoso)
				{
					return RedirectToAction(nameof(IndexNumeroVilla));
				}
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var res = await _villaService.ObtenerTodos<APIResponse>();

			if (res != null && res.IsExitoso) //en el caso de que algo falle, te carga de nuevo la lista de villas.
			{
				modelo.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Resultado))
											.Select(v => new SelectListItem
											{
												Text = v.Nombre,
												Value = v.Id.ToString(),
											}); //de esta forma llenas la lista de villa lista
			}
			return View(modelo);
		}

	}
}

using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_Utilidad;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using Newtonsoft.Json;
using System.Reflection;

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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexNumeroVilla()
		{
			List<NumeroVillaDto> numeroVillaList = new();

			var response = await _numeroVillaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

			if (response != null && response.IsExitoso)
			{
				numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDto>>(Convert.ToString(response.Resultado));
			}

			return View(numeroVillaList);
		}

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearNumeroVilla()
		{
			NumeroVillaViewModel numeroVillaVM = new();

			var response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

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
				var response = await _numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
				if (response != null && response.IsExitoso)
				{
					TempData["exitoso"] = "Numero Villa Creada Exitosamente";
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

			var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

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
        
		[Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarNumeroVilla(int villaNo)
		{
			NumeroVillaUpdateViewModel numeroVillaVM = new();

			var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));

			if (response != null && response.IsExitoso)
			{
				NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
				numeroVillaVM.NumeroVilla = _mapper.Map<NumeroVillaUpdateDto>(modelo);
			}

			response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

			if (response != null && response.IsExitoso) //en el caso de que algo falle, te carga de nuevo la lista de villas.
			{
				numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
											.Select(v => new SelectListItem
											{
												Text = v.Nombre,
												Value = v.Id.ToString(),
											}); //de esta forma llenas la lista de villa lista
				return View(numeroVillaVM);
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ActualizarNumeroVilla(NumeroVillaUpdateViewModel modelo)
		{
            if (ModelState.IsValid)
            {
                var response = await _numeroVillaService.Actualizar<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if (response != null && response.IsExitoso)
                {
					TempData["exitoso"] = "Numero Villa Actualizada Exitosamente";
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

            var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoverNumeroVilla(int villaNo)
        {
            NumeroVillaDeleteViewModel numeroVillaVM = new();

            var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsExitoso)
            {
                NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = modelo;
            }

            response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsExitoso) //en el caso de que algo falle, te carga de nuevo la lista de villas.
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
                                            .Select(v => new SelectListItem
                                            {
                                                Text = v.Nombre,
                                                Value = v.Id.ToString(),
                                            }); //de esta forma llenas la lista de villa lista
                return View(numeroVillaVM);
            }

            return NotFound();
        }

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> RemoverNumeroVilla(NumeroVillaDeleteViewModel modelo)
		{
			var response = await _numeroVillaService.Remover<APIResponse>(modelo.NumeroVilla.VillaNo, HttpContext.Session.GetString(DS.SessionToken));
			if (response != null && response.IsExitoso)
			{
				TempData["exitoso"] = "Numero Villa Removido Exitosamente";
				return RedirectToAction(nameof(IndexNumeroVilla));
			}
			TempData["error"] = "Ocurrio un error al remover Numero de Villa";
			return View(modelo);
		}


    }
}

﻿using AutoMapper;
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

		public async Task<IActionResult> CrearVilla() //llama a la vista
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CrearVilla(VillaCreateDto modelo) //envia los datos
		{
			if (ModelState.IsValid)
			{
				var response = await _villaService.Crear<APIResponse>(modelo);

				if (response != null && response.IsExitoso)
				{
					return RedirectToAction(nameof(IndexVilla));
				}
			}
			return View(modelo);
		}

		//Este método se utiliza para mostrar la vista de actualización de una villa específica.
		public async Task<IActionResult> ActualizarVilla(int villaId)
		{
			var response = await _villaService.Obtener<APIResponse>(villaId);

			if (response != null && response.IsExitoso)
			{
				VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Resultado));
				return View(_mapper.Map<VillaUpdateDto>(model));
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ActualizarVilla(VillaUpdateDto modelo)
		{//Este método es llamado cuando el usuario ha enviado el formulario con los datos modificados de la villa.
		 //Su función es procesar la actualización de la villa.
			if (ModelState.IsValid)
			{
				var response = await _villaService.Actualizar<APIResponse>(modelo);

				if (response != null && response.IsExitoso)
				{
					return RedirectToAction(nameof(IndexVilla));
				}
			}
			return View(modelo);
		}


		public async Task<IActionResult> RemoverVilla(int villaId)
		{
			var response = await _villaService.Obtener<APIResponse>(villaId);

			if (response != null && response.IsExitoso)
			{
				VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Resultado));
				return View(model);
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoverVilla(VillaDto modelo)
		{//Este método es llamado cuando el usuario ha enviado el formulario con los datos modificados de la villa.
		 //Su función es procesar la actualización de la villa.
			var response = await _villaService.Remover<APIResponse>(modelo.Id);

			if (response != null && response.IsExitoso)
			{
				return RedirectToAction(nameof(IndexVilla));
			}

			return View(modelo);
		}

	}
}
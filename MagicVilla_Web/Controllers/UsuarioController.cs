using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

namespace MagicVilla_Web.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
			_usuarioService = usuarioService;
        }

        public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDto modelo)
		{
			var response = await _usuarioService.Login<APIResponse>(modelo);
			if (response != null && response.IsExitoso == true)
			{
				LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Resultado));

				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(loginResponse.Token);

				//Claims - para guardar el USERNAME y el ROL - antes agregar los SERVICIOS + pipline de AUTENTICACION
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c=>c.Type=="unique_name").Value));
				identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				//Session
				HttpContext.Session.SetString(DS.SessionToken, loginResponse.Token); //esa sesion con ese nombre, y el contenido
				return RedirectToAction("Index", "Home");                           // del token es el que nos devuelve la API
			}
			else
			{
				ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
				return View(modelo); //vuelvo a retornar el modelo en caso de un error
			}
			
		}

		public IActionResult Registrar()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registrar(RegistroRequestDto modelo)
		{
			var response = await _usuarioService.Registrar<APIResponse>(modelo);

			if (response != null && response.IsExitoso)
			{
				return RedirectToAction("login");
			}

			return View();
		}

		public async Task<IActionResult> Logout() 
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(DS.SessionToken, "");

			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccesoDenegado()
		{
			return View();
		}

	}
}

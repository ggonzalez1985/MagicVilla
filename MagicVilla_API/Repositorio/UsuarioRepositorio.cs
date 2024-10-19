using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositorio
{
	public class UsuarioRepositorio : IUsuarioRepositorio
	{
		private readonly AplicationDbContext _db;
		private string secretKey;
		private readonly UserManager<UsuarioAplicacion> _userManager;
		private readonly IMapper _mapper;

        public UsuarioRepositorio(AplicationDbContext db, IConfiguration configuration, UserManager<UsuarioAplicacion> userManager,
            IMapper mapper)
        {
			_db = db;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
			_userManager = userManager;
			_mapper = mapper;
        }

        public bool IsUsuarioUnico(string UserName)
		{
			var usuario = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName.ToLower() == UserName.ToLower());
			if (usuario == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var usuario = await _db.UsuariosAplicacion.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

			bool isValido = await _userManager.CheckPasswordAsync(usuario, loginRequestDTO.Password); //controlar el pass con Identity

			if (usuario == null || isValido == false)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					Usuario = null,
				};
			}
			//si el Usuario Existe Generamos el JW Token
			var roles = await _userManager.GetRolesAsync(usuario); //capturo el rol del usuario
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, usuario.Id.ToString()),
					new Claim(ClaimTypes.Role, roles.FirstOrDefault())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new()
			{
				Token = tokenHandler.WriteToken(token),
				Usuario = _mapper.Map<UsuarioDto>(usuario),
				Rol = roles.FirstOrDefault()
			};
			return loginResponseDTO;
		}

		public async Task<UsuarioDto> Registrar(RegistroRequestDTO registroRequestDTO)
		{
			UsuarioAplicacion usuario = new()
			{
				UserName = registroRequestDTO.UserName,
				Email = registroRequestDTO.UserName,
				NormalizedEmail = registroRequestDTO.UserName.ToUpper(),
				Nombres = registroRequestDTO.Nombres,
			};

			try
			{
				var resultado = await _userManager.CreateAsync(usuario, registroRequestDTO.Password);
				if (resultado.Succeeded)
				{
					await _userManager.AddToRoleAsync(usuario, "admin");
					var usuarioAp = _db.UsuariosAplicacion.FirstOrDefault(u=>u.UserName == registroRequestDTO.UserName);
					return _mapper.Map<UsuarioDto>(usuarioAp);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return new UsuarioDto();
		}
	}
}

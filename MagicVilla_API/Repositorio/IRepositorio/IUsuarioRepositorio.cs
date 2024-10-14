using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Repositorio.IRepositorio
{
	public interface IUsuarioRepositorio
	{
		bool IsUsuarioUnico(string UserName); //verifica si el usuario es unico. no hay otro con el mismo user.

		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO); //el metodo de login

		Task<Usuario> Registrar(RegistroRequestDTO reguistroRequestDTO);
	}
}

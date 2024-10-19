namespace MagicVilla_API.Modelos.Dto
{
	public class LoginResponseDTO
	{
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; } //devuelve un token de autorizacion cuando el user hace login
        public string Rol { get; set; }
    }
}

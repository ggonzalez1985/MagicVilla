namespace MagicVilla_API.Modelos.Dto
{
	public class LoginResponseDTO
	{
        public Usuario Usuario { get; set; }
        public string Token { get; set; } //devuelve un token de autorizacion cuando el user hace login
    }
}

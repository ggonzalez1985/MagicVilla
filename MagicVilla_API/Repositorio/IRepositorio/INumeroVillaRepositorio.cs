using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
    {
        //Solamente el metodo de ACTUALIZACION
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}

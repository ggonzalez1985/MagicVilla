using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        //Solamente el metodo de ACTUALIZACION
        Task<Villa> Actualizar(Villa entidad);
    }
}

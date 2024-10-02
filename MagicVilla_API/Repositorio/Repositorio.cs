using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //aca voy a usar el bdContext y no en el controlador.
        private readonly AplicationDbContext _db;
        internal DbSet<T> dbSet; // Aquí vamos a almacenar el conjunto de entidades

        public Repositorio(AplicationDbContext db)
        {
            _db = db; // Guardamos el contexto de la base de datos
            this.dbSet = _db.Set<T>(); // Inicializamos dbSet para el tipo T
        }

        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad); // Agrega la entidad al conjunto en memoria
            await Grabar(); // Guarda los cambios en la base de datos
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet; // Inicia la consulta con el conjunto de entidades

            if (!tracked)
                query = query.AsNoTracking();

            if(filtro != null)
                query = query.Where(filtro); // Aplica un filtro, si se proporciona

            return await query.FirstOrDefaultAsync(); // Devuelve la primera entidad que cumple con el filtro
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
                query = query.Where(filtro);

            return await query.ToListAsync(); //Utilizas IQueryable<Villa> para construir una consulta
        }

        public async Task Remove(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }

        //no hay UPDATE xq en este metodo hay que saber que propiedad de una entidad especifica. 
        //no puede usarse un metodo generico para hacer update.

    }
}

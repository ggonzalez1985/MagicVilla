﻿using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {//T significa que podes recibir cualquier tipo de entidad
        Task Crear(T entidad);

        Task <List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro=null, string? incluirPropiedades=null);

        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked=true, string? incluirPropiedades = null);

        Task Remove(T entidad);

        Task Grabar();
    }
}

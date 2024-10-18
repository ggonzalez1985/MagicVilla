namespace MagicVilla_API.Modelos.Especificaciones
{
	public class PagedList<T> : List<T> //esta clase hace la paginacion, con los saltos y los cortes
	{
		public MetaData MetaData { get; set; }

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			MetaData = new MetaData
			{
				TotalCount = count,
				PageSize = pageSize,
				TotalPages = (int)Math.Ceiling(count / (double) pageSize) //por ejemplo 1.5 lo transforma a 2
			};
			AddRange(items);
		}

		public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int pageNumber, int pageSize)
		{
			var count = entidad.Count();
			var items = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}

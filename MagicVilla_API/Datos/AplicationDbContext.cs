using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class AplicationDbContext :DbContext
    {
        //con esto toma la connection string que especifique en "Program.cs"
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) :base(options)
        {
                
        }

        //en la BD el te crea esta tabla que especificas aca.
        public DbSet<Villa> Villas { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Villa>().HasData(
        //        new Villa()
        //        {
        //            Id = 1,
        //            Nombre = "Villa Real",
        //            Detalle = "Detalle de la villa...",
        //            ImagenUrl = "",
        //            Ocupantes = 5,
        //            MetrosCuadrados = 50,
        //            Tarifa = 200,
        //            Amenidad = "",
        //            FechaCreacion = DateTime.Now,
        //            FechaActualizacion = DateTime.Now
        //        },
        //        new Villa()
        //        {
        //            Id = 2,
        //            Nombre = "Villa Trucha",
        //            Detalle = "Detalle de la villa...",
        //            ImagenUrl = "",
        //            Ocupantes = 8,
        //            MetrosCuadrados = 80,
        //            Tarifa = 800,
        //            Amenidad = "",
        //            FechaCreacion = DateTime.Now,
        //            FechaActualizacion = DateTime.Now
        //        });
        //}
    }
}

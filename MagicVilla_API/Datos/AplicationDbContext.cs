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

    }
}

using MagicVilla_API.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MagicVilla_API.Datos
{
    public class AplicationDbContext :IdentityDbContext<UsuarioAplicacion>
    {
        //con esto toma la connection string que especifique en "Program.cs"
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) :base(options)
        {
                
        }

        //en la BD el te crea esta tabla que especificas aca.
        public DbSet<UsuarioAplicacion> UsuariosAplicacion { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> NumeroVillas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
        }

    }
}

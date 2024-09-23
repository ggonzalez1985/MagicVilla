using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class AplicationDbContext :DbContext
    {
        public DbSet<Villa> Villas { get; set; }
    }
}

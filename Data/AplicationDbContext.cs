using Microsoft.EntityFrameworkCore;

namespace bancontinental.Models
{
    public class AplicationDbContext: DbContext{
        public AplicationDbContext(DbContextOptions<AplicationDbContext> option) : base(option)
        {
            
        }

        public DbSet<Banco> bancos { get; set; }

        public DbSet<BancoCuenta> bancosCuentas { get; set; }

        public DbSet<BancoCuentaTrnsaccion> bancosCuentasTransacciones { get; set; }
    }
}
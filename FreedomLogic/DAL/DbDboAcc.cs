using FreedomLogic.Entities.Dbo;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbDboAcc : DbContext
    {
        public DbDboAcc(DbContextOptions<DbDboAcc> options)
            : base(options)
        {
        }
        public DbSet<DboAccount> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<GameAccount>()
            //    .HasOne(a => a.AccountAccess)
            //    .WithOne(r => r.Account);
        }
    }
}

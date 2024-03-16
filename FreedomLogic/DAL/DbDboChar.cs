using FreedomLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbDboChar : DbContext
    {
        public DbDboChar(DbContextOptions<DbDboChar> options)
            : base(options)
        {
        }
        public DbSet<DboCharacter> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<GameAccount>()
            //    .HasOne(a => a.AccountAccess)
            //    .WithOne(r => r.Account);
        }
    }
}

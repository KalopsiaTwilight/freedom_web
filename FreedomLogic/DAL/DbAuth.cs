using FreedomLogic.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbAuth : DbContext
    {
        public DbAuth(DbContextOptions<DbAuth> options)
            : base(options)
        {
        }
        public DbSet<Realmlist> Realmlists { get; set; }
        public DbSet<BnetAccount> BnetAccounts { get; set; }
        public DbSet<GameAccount> GameAccounts { get; set; }
        public DbSet<GameAccountAccess> GameAccountAccesses { get; set; }

        public DbSet<AccountBan> AccountBans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GameAccount>()
                .HasOne(a => a.AccountAccess)
                .WithOne(r => r.Account);
            builder.Entity<GameAccount>()
                .HasOne(a => a.BnetAccount)
                .WithMany(b => b.GameAccounts);
            builder.Entity<GameAccount>()
                .HasOne(a => a.AccountBan)
                .WithOne(b => b.GameAccount)
                .HasForeignKey<AccountBan>(x => x.Id)
                .IsRequired(false);
        }
    }
}

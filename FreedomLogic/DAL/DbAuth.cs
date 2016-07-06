using FreedomLogic.Entities;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DbAuth : DbContext
    {
        public DbAuth()
            : base("DBAuthContext")
        {
        }

        public DbSet<BnetAccount> BnetAccounts { get; set; }
        public DbSet<GameAccount> GameAccounts { get; set; }
        public DbSet<GameAccountAccess> GameAccountAccesses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameAccount>()
                .HasOptional(a => a.AccountAccess)
                .WithRequired(r => r.Account);
            modelBuilder.Entity<GameAccount>()
                .HasRequired(a => a.BnetAccount)
                .WithMany(r => r.GameAccounts);
        }
    }
}

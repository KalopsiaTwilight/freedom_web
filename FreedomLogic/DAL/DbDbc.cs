using FreedomLogic.Entities.Dbc;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbDbc : DbContext
    {
        public DbDbc(DbContextOptions<DbDbc> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemModifiedAppearance> ItemModifiedAppearances { get; set; }

        public DbSet<ItemAppearance> ItemAppearances { get; set; }
        public DbSet<ItemBonus> ItemBonuses { get; set; }
        public DbSet<SpellItemEnchantment> SpellItemEnchantments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(x => x.ItemModifiedAppearances)
                .WithOne(x => x.Item)
                .HasPrincipalKey(x => x.ID)
                .HasForeignKey(x => x.ItemID);

            modelBuilder.Entity<ItemModifiedAppearance>()
                .HasOne(x => x.ItemAppearance)
                .WithMany(x => x.ItemModifiedAppearance)
                .HasForeignKey(x => x.ItemAppearanceID)
                .HasPrincipalKey(x => x.ID);
        }
    }
}

using FreedomLogic.Entities.Characters;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbCharacters : DbContext
    {
        public DbCharacters(DbContextOptions<DbCharacters> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }

        public DbSet<CharacterCustomization> CharacterCustomizations { get; set; }
        public DbSet<CharacterInventorySlot> CharacterInventorySlots { get; set; }
        public DbSet<ItemInstance> ItemInstances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterCustomization>()
                .HasKey(x => new { x.CharacterId, x.CustomizationOptionId });

            modelBuilder.Entity<Character>()
                .HasMany(x => x.Customizations)
                .WithOne(x => x.Character)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.CharacterId);

            modelBuilder.Entity<Character>()
                .HasMany(x => x.InventorySlots)
                .WithOne(x => x.Character)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.CharacterId);

            modelBuilder.Entity<CharacterInventorySlot>()
                .HasOne(x => x.ItemInstance)
                .WithOne(x => x.InventorySlot)
                .HasForeignKey<CharacterInventorySlot>(x => x.ItemId)
                .HasPrincipalKey<ItemInstance>(x => x.Id);
                
            modelBuilder.Entity<Character>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Owner)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.OwnerId);

            modelBuilder.Entity<ItemInstanceTransmog>()
                .HasOne(x => x.ItemInstance)
                .WithOne(x => x.Transmog)
                .HasForeignKey<ItemInstanceTransmog>(x => x.ItemId)
                .HasPrincipalKey<ItemInstance>(x => x.Id);
        }
    }
}

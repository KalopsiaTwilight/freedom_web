using FreedomLogic.Entities;
using FreedomLogic.Identity;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbFreedom : DbContext
    {
        public DbFreedom(DbContextOptions<DbFreedom> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<FreedomRole> FreedomRoles { get; set; }
        public DbSet<ClassInfo> ClassInfos { get; set; }
        public DbSet<RaceInfo> RaceInfos { get; set; }
        public DbSet<MapInfo> MapInfos { get; set; }
        public DbSet<ZoneInfo> ZoneInfos { get; set; }
        public DbSet<CustomItemInfo> CustomItemInfos { get; set; }
        public DbSet<CreatureDisplayInfo> CreatureDisplayInfos { get; set; }
        public DbSet<ItemInventoryTypeInfo> ItemInventoryTypeInfos { get; set; }
        public DbSet<ItemClassInfo> ItemClassInfos { get; set; }
        public DbSet<ItemSubclassInfo> ItemSubclassInfos { get; set; }
        public DbSet<FreedomCommand> FreedomCommands { get; set; }
        public DbSet<GameobjectTypeInfo> GameobjectTypeInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User <-- Many-To-Many --> FreedomRole
            modelBuilder.Entity<FreedomRole>()
                 .HasMany(r => r.Users)
                 .WithMany(u => u.FreedomRoles)
                 .UsingEntity<UserRole>();

            modelBuilder.Entity<ItemSubclassInfo>()
                .HasKey(x => new { x.Id, x.SubclassId });
        }
    }
}

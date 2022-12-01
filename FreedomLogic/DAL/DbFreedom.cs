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
        public DbSet<ClassInfo> ClassInfos { get; set; }
        public DbSet<RaceInfo> RaceInfos { get; set; }
        public DbSet<MapInfo> MapInfos { get; set; }
        public DbSet<ZoneInfo> ZoneInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FreedomRole> Roles { get; set; }
        public DbSet<FreedomCommand> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User <-- Many-To-Many --> FreedomRole
            modelBuilder.Entity<FreedomRole>()
                 .HasMany(r => r.Users)
                 .WithMany(u => u.FreedomRoles)
                 .UsingEntity<UserRole>();

            modelBuilder.Entity<User>()
             .HasMany(u => u.FreedomRoles)
             .WithMany(r => r.Users)
             .UsingEntity<UserRole>();

            //modelBuilder.Entity<ItemSubclassInfo>()
            //    .HasKey(x => new { x.Id, x.SubclassId });
        }
    }
}

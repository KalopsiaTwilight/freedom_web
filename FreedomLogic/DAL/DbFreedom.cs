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

        public DbSet<ItemTemplateExtra> ItemTemplateExtras { get; set; }
        public DbSet<NpcItemInfo> NpcItemInfos { get; set; }
        public DbSet<ItemBonusIdInfo> ItemBonusIdInfos { get; set; }

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

            modelBuilder.Entity<ItemTemplateExtra>()
                .HasOne(ite => ite.NpcItemInfo)
                .WithOne(ite => ite.ItemTemplateExtra)
                .HasForeignKey<NpcItemInfo>(nii => nii.ItemId);

            modelBuilder.Entity<ItemTemplateExtra>()
                .HasOne(ite => ite.BonusIdInfo)
                .WithOne(ite => ite.ItemTemplateExtra)
                .HasForeignKey<ItemBonusIdInfo>(ibi =>ibi.ItemId);

            modelBuilder.Entity<ItemBonusIdInfo>()
                .HasKey(x => new { x.ItemAppearanceModifierId, x.ItemId });

            modelBuilder.Entity<ItemBonusIdInfo>()
                .HasOne(ibi => ibi.NpcItemInfo)
                .WithOne(nii => nii.BonusIdInfo)
                .HasForeignKey<NpcItemInfo>(nii => nii.ItemId)
                .HasPrincipalKey<ItemBonusIdInfo>(ibi => ibi.ItemId);

            //modelBuilder.Entity<ItemSubclassInfo>()
            //    .HasKey(x => new { x.Id, x.SubclassId });
        }
    }
}

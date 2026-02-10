using FreedomLogic.Entities.Freedom;
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

        public DbSet<CharacterExtra> CharacterExtras { get; set; }
        public DbSet<TextureFileData> TextureFiles { get; set; }
        public DbSet<ItemDisplayInfoData> ItemDisplayInfos { get; set; }
        public DbSet<ModelResourcesData> ModelResources { get; set; }
        public DbSet<ModelViewerModelData> ModelViewerModelData { get; set; }

        public DbSet<ModelViewerCollection> ModelViewerCollections { get; set; }
        public DbSet<ModelViewerModelToTag> ModelViewerModelToTag { get; set; }
        public DbSet<ModelViewerTag> ModelViewerTags { get; set; }
        public DbSet<ModelViewerModelToCollection> ModelViewerModelToCollection { get; set; }

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

            modelBuilder.Entity<ItemDisplayInfoData>()
                .HasKey(x => new { x.Id, x.DisplayId });

            modelBuilder.Entity<ModelViewerModelToTag>()
                .HasOne(x => x.Model)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<ModelViewerModelToTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.TagId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<ModelViewerModelToTag>()
                .HasOne(x => x.User)
                .WithMany(x => x.ModelTags)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<ModelViewerModelToTag>()
                .HasKey(x => new { x.TagId, x.UserId, x.ModelId });
            modelBuilder.Entity<ModelViewerCollection>()
                .HasOne(x => x.User)
                .WithMany(x => x.ModelViewerCollections)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<ModelViewerModelToCollection>()
                .HasOne(x => x.Collection)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.CollectionId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<ModelViewerModelToCollection>()
                .HasOne(x => x.Model)
                .WithMany(x => x.Collections)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<ModelViewerModelToCollection>()
                .HasKey(x => new { x.ModelId, x.CollectionId });

            //modelBuilder.Entity<ItemSubclassInfo>()
            //    .HasKey(x => new { x.Id, x.SubclassId });
        }
    }
}

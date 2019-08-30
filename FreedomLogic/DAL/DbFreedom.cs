using FreedomLogic.Entities;
using FreedomLogic.Identity;
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
    public class DbFreedom : DbContext
    {
        public DbFreedom()
            : base("DBFreedomContext")
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User <-- Many-To-Many --> FreedomRole
            modelBuilder.Entity<FreedomRole>()
                .HasMany(r => r.Users)
                .WithMany(u => u.FreedomRoles)
                .Map(m => 
                {
                    m.ToTable("user_roles");
                    m.MapLeftKey("id_role");
                    m.MapRightKey("id_user");
                });
        }

        public static DbFreedom Create()
        {
            return new DbFreedom();
        }
    }
}

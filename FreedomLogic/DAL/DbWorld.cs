using FreedomLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.DAL
{
    public class DbWorld : DbContext
    {
        public DbWorld(DbContextOptions<DbWorld> options)
            : base(options)
        {
        }
        public DbSet<CreatureTemplate> CreatureTemplates { get; set; }
        public DbSet<CreatureEquipTemplate> CreatureEquipTemplates { get; set; }
        public DbSet<GameobjectTemplate> GameobjectTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreatureTemplate>()
                .HasMany(e1 => e1.CreatureEquipTemplates)
                .WithOne(e2 => e2.CreatureTemplate);
        }
    }
}

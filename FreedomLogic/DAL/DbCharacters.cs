using FreedomLogic.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterCustomization>()
                .HasKey(x => new { x.CharacterId, x.CustomizationOptionId });
        }
    }
}

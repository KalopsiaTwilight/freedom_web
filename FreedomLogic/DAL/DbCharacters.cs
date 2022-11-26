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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

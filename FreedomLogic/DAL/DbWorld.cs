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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

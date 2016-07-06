using FreedomLogic.Entities;
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
    public class DbWorld : DbContext
    {
        public DbWorld()
            : base("DBWorldContext")
        {
        }

        public DbSet<CreatureTemplate> CreatureTemplates { get; set; }
        public DbSet<CreatureEquipTemplate> CreatureEquipTemplates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreatureTemplate>()
                .HasMany(e1 => e1.CreatureEquipTemplates)
                .WithRequired(e2 => e2.CreatureTemplate);
        }
    }
}

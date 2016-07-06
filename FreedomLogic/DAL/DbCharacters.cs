using FreedomLogic.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.DAL
{
    public class DbCharacters : DbContext
    {
        public DbCharacters()
            : base("DBCharactersContext")
        {
        }

        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

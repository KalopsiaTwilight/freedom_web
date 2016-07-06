using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    [Table("roles")]
    public class FreedomRole
    {
        [NotMapped]
        public const string RoleAdmin = "admin";

        [Column("id")]
        public int Id { get; set; }

        [Column("role_name")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

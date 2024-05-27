using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Identity
{
    [Table("roles")]
    public class FreedomRole
    {
        [NotMapped]
        public const string RoleAdmin = "admin";

        [NotMapped]
        public const string RoleGM = "gm";

        [Column("id")]
        public int Id { get; set; }

        [Column("role_name")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("user_roles")]
    internal class UserRole
    {
        [Column("id_role")]
        public int FreedomRoleId { get; set; }
        [Column("id_user")]
        public int UserId { get; set; }
    }
}

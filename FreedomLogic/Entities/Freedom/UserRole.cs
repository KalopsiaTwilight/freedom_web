using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
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

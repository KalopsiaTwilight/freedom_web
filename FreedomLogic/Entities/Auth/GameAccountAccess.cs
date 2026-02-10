using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Auth
{
    [Table("account_access")]
    public class GameAccountAccess : EntityBase
    {
        public GameAccountAccess()
        {
            RealmId = -1;
        }

        [Column("AccountId")]
        [Key]
        public override int Id { get; set; }
        
        [Column("SecurityLevel")]
        public GMLevel GMLevel { get; set; }

        [Column("RealmID")]
        public int RealmId { get; set; }

        [ForeignKey("Id")]
        public GameAccount Account { get; set; }
    }
}

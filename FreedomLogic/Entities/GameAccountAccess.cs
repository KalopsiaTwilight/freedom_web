using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
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

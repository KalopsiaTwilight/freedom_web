using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FreedomLogic.Managers.AccountManager;

namespace FreedomLogic.Entities
{
    [Table("account")]
    public class GameAccount : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("username")]
        [Required]     
        public string Username { get; set; }

        [Column("sha_pass_hash")]
        public string ShaPassHash { get; set; }

        [Column("expansion")]
        public GameExpansion Expansion { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("reg_mail")]
        public string RegEmail { get; set; }

        [Column("joindate")]
        public DateTime Joined { get; set; }

        [Column("battlenet_account")]
        public int BnetAccountId { get; set; }

        [Column("battlenet_index")]
        public byte BnetAccountIndex { get; set; }

        [ForeignKey("BnetAccountId")]
        public BnetAccount BnetAccount { get; set; }

        public GameAccountAccess AccountAccess { get; set; }
    }

    public class GameAccountIDs
    {
        public int ID { get; set; }
    }
}

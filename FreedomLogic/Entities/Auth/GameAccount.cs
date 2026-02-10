using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Auth
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

        [Column("salt")]
        public byte[] Salt { get; set; }
        [Column("verifier")]
        public byte[] Verifier { get; set; }

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
        public virtual AccountBan AccountBan { get; set; }
    }
}

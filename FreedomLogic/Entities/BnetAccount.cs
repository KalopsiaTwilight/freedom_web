using FreedomLogic.Infrastructure;
using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("battlenet_accounts")]
    public class BnetAccount : EntityBase
    {
        public BnetAccount()
        {
        }

        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("email")]
        [Required]
        public string UsernameEmail { get; set; }

        [Column("sha_pass_hash")]
        public string ShaPassHash { get; set; }

        [Column("online")]
        public bool IsOnline { get; set; }

        [Column("joindate")]
        public DateTime Joined { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        public List<GameAccount> GameAccounts { get; set; }
    }
}

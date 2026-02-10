using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities.Dbo
{
    public enum DboAccountLevel
    {
        [Display(Name = "No Account / Access")]
        NoAccess = -1,
        [Display(Name = "Player / None")]
        Player = 0,
        [Display(Name = "Game Master")]
        GameMaster = 2,
        [Display(Name = "Community Manager")]
        CommunityManager = 3,
        [Display(Name = "Administrator")]
        Admin = 10
    }

    [Table("accounts")]
    public class DboAccount
    {
        [Column("AccountId")]
        [Key]
        public int Id { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password_hash")]
        public string PasswordHash { get; set; }
        [Column("acc_status")]
        public string AccountStatus { get; set; } = "active";
        [Column("email")]
        public string Email { get; set; }
        [Column("mallpoints")]
        public int MallPoints { get; set; } = 10000000;
        [Column("reg_date")]
        public DateTime RegistrationDate { get; set; }
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
        [Column("reg_ip")]
        public string? RegistrationIp { get; set; }
        [Column("admin")]
        public DboAccountLevel AdminLevel2 { get; set; }
        [Column("isGm")]
        public DboAccountLevel AdminLevel { get; set; }
        [Column("lastServerFarmId")]
        public int LastServerFarmId { get; set; } = 255;
        [Column("founder")]
        public int Founder { get; set; }
        [Column("founder_recv")]
        public bool FounderReceived { get; set; }
        [Column("last_ip")]
        public string LastKnownIp { get; set; } = "0.0.0.0";
        [Column("del_char_pw")]
        public string DelCharPw { get; set; } = "25f9e794323b453885f5181f1b624d0b";
        [Column("PremiumSlots")]
        public int PremiumSlots { get; set; } = 4;
        [Column("EventCoins")]
        public int EventCoins { get; set; }
        [Column("WaguCoins")]
        public int WaguCoins { get; set; }
        [Column("web_ip")]
        public string? WebIp { get; set; }
    }
}

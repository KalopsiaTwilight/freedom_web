using FreedomLogic.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FreedomLogic.Entities
{
    [Table("account_banned")]
    public class AccountBan: EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }


        [Column("bandate")]
        public long BanDateUnixTimestamp { get; set; }

        [NotMapped]
        public DateTimeOffset BanDate
        {
            get => DateTimeOffset.FromUnixTimeSeconds(BanDateUnixTimestamp);
            set => BanDateUnixTimestamp = value.ToUnixTimeSeconds();
        }


        [Column("unbandate")]
        public long UnbanDateUnixTimestamp { get; set; }

        [NotMapped]
        public DateTimeOffset Unbandate
        {
            get => DateTimeOffset.FromUnixTimeSeconds(UnbanDateUnixTimestamp);
            set => UnbanDateUnixTimestamp = value.ToUnixTimeSeconds();
        }

        [Column("bannedby")]
        public string BannedBy { get; set; }
        [Column("banreason")]
        public string BanReason { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        public virtual GameAccount GameAccount { get; set; }
    }
}

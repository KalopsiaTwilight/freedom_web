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
    [Table("characters")]
    public class Character : EntityBase
    {
        [Column("guid")]
        [Key]
        public override int Id { get; set; }

        [Column("account")]
        public int GameAccountId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("class")]
        public CharClass Class { get; set; }

        [Column("gender")]
        public CharGender Gender { get; set; }

        [Column("race")]
        public CharRace Race { get; set; }

        [Column("map")]
        public short MapId { get; set; }

        [Column("zone")]
        public short ZoneId { get; set; }

        [Column("online")]
        public bool IsOnline { get; set; }

        [Column("extra_flags")]
        public short ExtraFlags { get; set; }

        [Column("deleteDate")]
        public DateTime? DeleteDate { get; set; }

        [Column("latency")]
        public int Latency { get; set; }

        [NotMapped]
        public CharData CharData
        {
            get
            {
                var charData = new CharData();

                if (charData.Load(this.Id))
                    return charData;
                else
                    return null;
            }
        }
    }
}

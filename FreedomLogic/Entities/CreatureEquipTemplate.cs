using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("creature_equip_template")]
    public class CreatureEquipTemplate : EntityBase
    {
        [Key]
        [Column("CreatureID")]
        public override int Id { get; set; }

        [Column("ItemID1")]
        public int ItemIdMainHand { get; set; }

        [Column("ItemID2")]
        public int ItemIdOffHand { get; set; }

        [Column("ItemID3")]
        public int ItemIdRanged { get; set; }

        [ForeignKey("Id")]
        public CreatureTemplate CreatureTemplate { get; set; }
    }
}

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
    [Table("creature_display_info")]
    public class CreatureDisplayInfo : EntityBase
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("id_itemdisplay_head")]
        public int ItemDisplayHead { get; set; }

        [Column("id_itemdisplay_shoulder")]
        public int ItemDisplayShoulder { get; set; }

        [Column("id_itemdisplay_shirt")]
        public int ItemDisplayShirt { get; set; }

        [Column("id_itemdisplay_cuirass")]
        public int ItemDisplayCuirass { get; set; }

        [Column("id_itemdisplay_belt")]
        public int ItemDisplayBelt { get; set; }

        [Column("id_itemdisplay_legs")]
        public int ItemDisplayLegs { get; set; }

        [Column("id_itemdisplay_boots")]
        public int ItemDisplayBoots { get; set; }

        [Column("id_itemdisplay_wrist")]
        public int ItemDisplayWrist { get; set; }

        [Column("id_itemdisplay_gloves")]
        public int ItemDisplayGloves { get; set; }

        [Column("id_itemdisplay_tabard")]
        public int ItemDisplayTabard { get; set; }

        [Column("id_itemdisplay_cape")]
        public int ItemDisplayCape { get; set; }
    }
}

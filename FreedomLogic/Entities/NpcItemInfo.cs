using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities
{
    [Table("npc_to_itemid")]
    public class NpcItemInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("creatureid")]
        public int CreatureId { get; set; }

        [Column("creaturename")]
        public string CreatureName { get; set; }

        [Column("itemid")]
        public int ItemId { get; set; }

        [Column("itemappearancemodifierid")]
        public int ItemAppearanceModifierId { get; set; }

        public virtual ItemBonusIdInfo BonusIdInfo { get; set; }
        public virtual ItemTemplateExtra ItemTemplateExtra { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
{
    [Table("item_to_bonusid")]
    public class ItemBonusIdInfo
    {
        [Column("itemid")]
        public int ItemId { get; set; }

        [Column("itemappearancemodifierid")]
        public byte ItemAppearanceModifierId { get; set; }

        [Column("bonusid")]
        public int BonusId { get; set; }

        [Column("itemname")]
        public string ItemName { get; set; }

        public virtual ItemTemplateExtra ItemTemplateExtra { get; set; }
        public virtual NpcItemInfo NpcItemInfo { get; set; }
    }
}

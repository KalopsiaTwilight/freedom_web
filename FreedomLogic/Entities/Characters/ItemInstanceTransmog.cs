using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Characters
{

    [Table("item_instance_transmog")]
    public class ItemInstanceTransmog
    {
        [Column("itemGuid")]
        [Key]
        public int ItemId { get; set; }
        [Column("itemModifiedAppearanceAllSpecs")]
        public int ItemModifiedAppearanceAllSpecs { get; set; }
        [Column("itemModifiedAppearanceSpec1")]
        public int ItemModifiedAppearanceSpec1 { get; set; }
        [Column("itemModifiedAppearanceSpec2")]
        public int ItemModifiedAppearanceSpec2 { get; set; }
        [Column("itemModifiedAppearanceSpec3")]
        public int ItemModifiedAppearanceSpec3 { get; set; }
        [Column("itemModifiedAppearanceSpec4")]
        public int ItemModifiedAppearanceSpec4 { get; set; }
        [Column("itemModifiedAppearanceSpec5")]
        public int ItemModifiedAppearanceSpec5 { get; set; }
        [Column("spellItemEnchantmentAllSpecs")]
        public int SpellItemEnchantmentAllSpecs { get; set; }
        [Column("spellItemEnchantmentSpec1")]
        public int SpellItemEnchantmentSpec1 { get; set; }
        [Column("spellItemEnchantmentSpec2")]
        public int SpellItemEnchantmentSpec2 { get; set; }
        [Column("spellItemEnchantmentSpec3")]
        public int SpellItemEnchantmentSpec3 { get; set; }
        [Column("spellItemEnchantmentSpec4")]
        public int SpellItemEnchantmentSpec4 { get; set; }
        [Column("spellItemEnchantmentSpec5")]
        public int SpellItemEnchantmentSpec5 { get; set; }
        [Column("secondaryItemModifiedAppearanceAllSpecs")]
        public int SecondaryItemModifiedAppearanceAllSpecs { get; set; }
        [Column("secondaryItemModifiedAppearanceSpec1")]
        public int SecondaryItemModifiedAppearanceSpec1 { get; set; }
        [Column("secondaryItemModifiedAppearanceSpec2")]
        public int SecondaryItemModifiedAppearanceSpec2 { get; set; }
        [Column("secondaryItemModifiedAppearanceSpec3")]
        public int SecondaryItemModifiedAppearanceSpec3 { get; set; }
        [Column("secondaryItemModifiedAppearanceSpec4")]
        public int SecondaryItemModifiedAppearanceSpec4 { get; set; }
        [Column("secondaryItemModifiedAppearanceSpec5")]
        public int SecondaryItemModifiedAppearanceSpec5 { get; set; }

        public ItemInstance ItemInstance { get; set; }
    }
}
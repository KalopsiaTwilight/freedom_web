using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbc
{
    [Table("spellitemenchantment")]
    public class SpellItemEnchantment
    {
        [Key]
        public int ID { get; set; }
        public string Name_lang { get; set; }
        public string HordeName_lang { get; set; }
        public uint EffectArg0 { get; set; }
        public uint EffectArg1 { get; set; }
        public uint EffectArg2 { get; set; }
        public float EffectScalingPoints0 { get; set; }
        public float EffectScalingPoints1 { get; set; }
        public float EffectScalingPoints2 { get; set; }
        public uint IconFileDataID { get; set; }
        public int ItemLevelMin { get; set; }
        public int ItemLevelMax { get; set; }
        public uint TransmogUseConditionID { get; set; }
        public uint TransmogCost { get; set; }
        public short EffectPointsMin0 { get; set; }
        public short EffectPointsMin1 { get; set; }
        public short EffectPointsMin2 { get; set; }
        public ushort ItemVisual { get; set; }
        public ushort Flags { get; set; }
        public ushort RequiredSkillID { get; set; }
        public ushort RequiredSkillRank { get; set; }
        public ushort ItemLevel { get; set; }
        public byte Charges { get; set; }
        public byte Effect0 { get; set; }
        public byte Effect1 { get; set; }
        public byte Effect2 { get; set; }
        public sbyte ScalingClass { get; set; }
        public sbyte ScalingClassRestricted { get; set; }
        public byte Condition_ID { get; set; }
        public byte MinLevel { get; set; }
        public byte MaxLevel { get; set; }
    }
}

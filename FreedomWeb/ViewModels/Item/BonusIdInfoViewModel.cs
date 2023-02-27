using FreedomLogic.Entities;

namespace FreedomWeb.ViewModels.Item
{
    public class BonusIdInfoViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public byte ItemAppearanceModifierId { get; set; }
        
        public int BonusId { get; set; }
    }
}

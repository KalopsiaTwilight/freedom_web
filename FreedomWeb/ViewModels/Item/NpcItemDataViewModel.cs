using FreedomLogic.Entities.Freedom;

namespace FreedomWeb.ViewModels.Item
{
    public class NpcItemDataViewModel
    {
        public int CreatureId { get; set; }
        public string CreatureName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public InventoryType ItemInventoryType { get; set; }
    }
}

using System.Collections.Generic;

namespace FreedomWeb.Models
{
    public enum WoWInventoryType
    {

    }

    public class CustomItemData
    {
        public int InventoryType { get; set; }
        public Dictionary<int, CustomItemItemMaterialData> ItemMaterials { get; set; }
        public Dictionary<string, CustomItemComponentModelData> ItemComponentModels { get; set; }
        public int[][] ParticleColors { get; set; }
        public List<CustomItemHelmetGeoVisData> HelmetGeoVisFemale { get; set; }
        public List<CustomItemHelmetGeoVisData> HelmetGeoVisMale { get; set; }
        public int Flags { get; set; }  
        public int[] GeoSetGroup { get; set; }
    }

    public class CustomItemItemMaterialData
    {
        public string FileName {get; set; }
        public int FileId { get; set; }
        public int Gender { get; set; }
        public int Race { get; set; }
        public int Class { get; set; }
    }

    public class CustomItemComponentModelData
    {
        public CustomItemComponentModelTextureData Texture { get; set; }
        public List<CustomItemComponentModelModelData> Models { get; set; }
    }

    public class CustomItemComponentModelTextureData
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class CustomItemComponentModelModelData
    {
        public string FilaeName { get; set; }
        public int FileId { get; set; }
        public int Race { get; set; } = 0;
        public int Class { get; set; } = 0;
        public int Gender { get; set; } = 0;
        public int ExtraData { get; set; } = 0;
    }


    public class CustomItemHelmetGeoVisData
    {
        public int Race { get; set; } = 0;
        public int Group { get; set; } = 0;
    }
}

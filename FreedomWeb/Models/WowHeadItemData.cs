using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreedomWeb.Models
{
    public class WowHeadItemData
    {
        [JsonPropertyName("Model")]
        public int Model { get; set; }
        [JsonPropertyName("Textures")]
        public Dictionary<string, int> Textures { get; set; }
        [JsonPropertyName("Textures2")]
        public Dictionary<string, int> Textures2 { get; set; }

        [JsonPropertyName("TextureFiles")]
        public Dictionary<int, List<WoWHeadItemDataFileData>> TextureFiles { get; set; }
        [JsonPropertyName("ModelFiles")]
        public Dictionary<int, List<WoWHeadItemDataFileData>> ModelFiles { get; set; }
        [JsonPropertyName("Item")]
        public WowHeadItemDataItem Item { get; set; }

        [JsonPropertyName("ComponentTextures")]
        public Dictionary<int, string> ComponentTextures { get; set; }
        [JsonPropertyName("ComponentModels")]
        public Dictionary<int, int> ComponentModels { get; set; }

        [JsonPropertyName("Creature")]
        public object? Creature { get; set; }
        [JsonPropertyName("Character")]
        public object? Character { get; set; }
        [JsonPropertyName("ItemEffects")]
        public object? ItemEffects { get; set; }
        [JsonPropertyName("Equipment")]
        public object? Equipment { get; set; }
        [JsonPropertyName("StateKit")]
        public object? StateKit { get; set; }
        [JsonPropertyName("StateKits")]
        public object? StateKits { get; set; }
        [JsonPropertyName("Scale")]
        public double Scale { get; set; }
    }

    public class WowHeadItemDataItem
    {
        [JsonPropertyName("Flags")]
        public int Flags { get; set; }
        [JsonPropertyName("InventoryType")]
        public int InventoryType { get; set; }
        [JsonPropertyName("ItemClass")]
        public int ItemClass { get; set; }
        [JsonPropertyName("ItemSubClass")]
        public int ItemSubClass { get; set; }
        [JsonPropertyName("HideGeosetMale")]
        public List<CustomItemHelmetGeoVisData>? HideGeosetMale { get; set; }
        [JsonPropertyName("HideGeosetFemale")]
        public List<CustomItemHelmetGeoVisData>? HideGeosetFemale { get; set; }
        [JsonPropertyName("GeosetGroup")]
        public int[] GeosetGroup { get; set; }
        [JsonPropertyName("AttachGeosetGroup")]
        public int[] AttachGeosetGroup { get; set; }
        [JsonPropertyName("GeosetGroupOverride")]
        public int GeosetGroupOverride { get; set; }
        [JsonPropertyName("ParticleColor")]
        public WowHeadItemDataParticleColor? ParticleColor { get; set; }
    }

    public class WowHeadItemDataGeosetData
    {
        [JsonPropertyName("RaceId")]
        public int RaceId { get; set; }
        [JsonPropertyName("GeosetGroup")]
        public int GeosetGroup { get; set; }
        [JsonPropertyName("RaceBitSelection")]
        public int RaceBitSelection { get; set; }
    }

    public class WowHeadItemDataParticleColor
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Start")]
        public long[] Start { get; set; }
        [JsonPropertyName("Mid")]
        public long[] Mid { get; set; }
        [JsonPropertyName("End")]
        public long[] End { get; set; }
    }

    public class WoWHeadItemDataFileData
    {
        [JsonPropertyName("FileDataId")]
        public int FileDataId { get; set; }
        [JsonPropertyName("Gender")]
        public int Gender { get; set; }
        [JsonPropertyName("Race")]
        public int Race { get; set; }
        [JsonPropertyName("Class")]
        public int Class { get; set; }
        [JsonPropertyName("ExtraData")]
        public int ExtraData { get; set; }

    }
}

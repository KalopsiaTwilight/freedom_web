using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.Models
{
    public class CustomItemResultListItem
    {
        public int EntryId { get; set; }

        public int DisplayId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Class { get; set; }

        public string ClassName { get; set; }

        public int SubClass { get; set; }

        public string SubClassName { get; set; }

        public int InventoryType { get; set; }

        public string InventoryTypeName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.Models
{
    public class GameobjectResultListItem
    {
        public int EntryId { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int DisplayId { get; set; }

        public string Name { get; set; }

        public float Size { get; set; }
    }
}
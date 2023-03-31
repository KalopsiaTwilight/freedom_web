using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.Models
{
    public class StatusCharListItem
    {
        public int UserId { get; set; }

        public string FactionIconPath { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public string OwnerUsername { get; set; }

        public string Race { get; set; }

        public string RaceIconPath { get; set; }

        public string Class { get; set; }

        public string ClassIconPath { get; set; }

        public string Gender { get; set; }

        public string MapName { get; set; }

        public string ZoneName { get; set; }

        public int Latency { get; set; }

        public string Phase { get; set; }
    }
}
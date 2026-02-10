using FreedomLogic.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreedomUtils.MvcUtils;
using Newtonsoft.Json;

namespace FreedomLogic.Entities.Freedom
{
    [Table("commands")]
    [Serializable]
    public class FreedomCommand
    {
        [Key]
        [Column("command")]
        public string Command { get; set; }

        [Column("syntax")]
        public string Syntax { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("gmlevel")]
        [JsonIgnore]
        public GMLevel GMLevel { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "GMLevel")]
        public string GMLevelDisplay
        {
            get { return GMLevel.DisplayName(); }
        }
    }
}

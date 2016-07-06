using FreedomLogic.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("commands")]
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
        public GMLevel GMLevel { get; set; }
    }
}

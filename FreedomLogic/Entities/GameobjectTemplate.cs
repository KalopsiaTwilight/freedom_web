using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("gameobject_template")]
    public class GameobjectTemplate : EntityBase
    {
        [Key]
        [Column("entry")]
        public override int Id { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("displayId")]
        public int DisplayId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("size")]
        public float Size { get; set; }
    }
}

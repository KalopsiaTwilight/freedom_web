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
    [Table("creature_template")]
    public class CreatureTemplate : EntityBase
    {
        [Key]
        [Column("entry")]
        public override int Id { get; set; }

        [Column("modelid1")]
        public int ModelId1 { get; set; }

        [Column("modelid2")]
        public int ModelId2 { get; set; }

        [Column("modelid3")]
        public int ModelId3 { get; set; }

        [Column("modelid4")]
        public int ModelId4 { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("subname")]
        public string Subname { get; set; }

        public List<CreatureEquipTemplate> CreatureEquipTemplates { get; set; }
    }
}

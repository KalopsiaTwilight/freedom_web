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
    [Table("class_info")]
    public class ClassInfo : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("icon")]
        public string IconPath { get; set; }
    }
}

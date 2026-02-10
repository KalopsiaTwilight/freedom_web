using FreedomLogic.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
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

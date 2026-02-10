using FreedomLogic.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FreedomLogic.Entities.Freedom
{
    [Table("modelresources")]
    public class ModelResourcesData : EntityBase
    {
        [Key]
        [Column("fileId")]
        public override int Id { get; set; }
        [Column("fileName")]
        public string FileName { get; set; }
    }
}

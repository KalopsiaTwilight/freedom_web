using FreedomLogic.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FreedomLogic.Entities.Freedom
{
    [Table("texturefiles")]
    public class TextureFileData : EntityBase
    {
        [Column("fileId")]
        [Key]
        public override int Id { get; set; }
        [Column("fileName")]
        public string FileName { get; set; }
    }
}

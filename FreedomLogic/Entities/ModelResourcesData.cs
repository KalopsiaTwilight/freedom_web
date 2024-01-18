using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
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

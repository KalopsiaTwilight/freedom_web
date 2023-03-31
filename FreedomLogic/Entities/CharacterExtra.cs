using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("character_extra")]
    public class CharacterExtra
    {
        [Column("guid")]
        [Key]
        public int Id { get; set; }

        [Column("phase")]
        public int Phase { get; set; }
    }
}

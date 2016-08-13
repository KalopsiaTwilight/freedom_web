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
    [Table("realmlist")]
    public class Realmlist : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("localAddress")]
        public string LocalAddress { get; set; }

        [Column("localSubnetMask")]
        public string LocalSubnetMask { get; set; }

        [Column("port")]
        public short Port { get; set; }

        [Column("icon")]
        public byte Icon { get; set; }

        [Column("flag")]
        public byte Flags { get; set; }

        [Column("timezone")]
        public byte Timezone { get; set; }

        [Column("allowedSecurityLevel")]
        public byte AllowedSecurityLevel { get; set; }

        [Column("population")]
        public float Population { get; set; }

        [Column("gamebuild")]
        public int GameBuild { get; set; }

        [Column("Region")]
        public byte Region { get; set; }

        [Column("Battlegroup")]
        public byte Battlegroup { get; set; }
    }
}

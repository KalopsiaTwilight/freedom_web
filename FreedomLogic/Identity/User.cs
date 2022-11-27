using FreedomLogic.Entities;
using FreedomLogic.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    [Table("users")]
    public class User 
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("id_battlenet_account")]
        public int BnetAccountId { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("bnet_sha256_pass_hash")]
        public string BnetAccPassHash { get; set; }

        [NotMapped]
        public string GameAccPassHash { get; set; }

        [Column("displayname")]
        public string DisplayName { get; set; }

        [Column("registered_email")]
        public string RegEmail { get; set; }

        [Column("email_confirm")]
        public bool EmailConfirmed { get; set; }

        [Column("security_stamp")]
        public string SecurityStamp { get; set; }

        public ICollection<FreedomRole> FreedomRoles { get; set; }

        [NotMapped]
        public UserData UserData { get; set; }
    }
}

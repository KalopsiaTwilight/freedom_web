using FreedomLogic.Entities;
using FreedomLogic.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class User : EntityBase, IUser<int>
    {        
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("id_battlenet_account")]
        public int BnetAccountId { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("bnet_sha256_pass_hash")]
        public string BnetAccPassHash { get; set; }

        [Column("game_sha1_pass_hash")]
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [NotMapped]
        public UserData UserData
        {
            get
            {
                var userData = new UserData();

                if (userData.Load(this.Id))
                    return userData;
                else
                    return null;
            }
        }
    }
}

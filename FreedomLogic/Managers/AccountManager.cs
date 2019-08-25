using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using FreedomLogic.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    public enum GMLevel
    {
        [Display(Name = "FieldGMLevelPlayer", ResourceType = typeof(AccountRes))]
        Player = 0,
        [Display(Name = "FieldGMLevelModerator", ResourceType = typeof(AccountRes))]
        Moderator = 1,
        [Display(Name = "FieldGMLevelGM", ResourceType = typeof(AccountRes))]
        GameMaster = 2,
        [Display(Name = "FieldGMLevelAdmin", ResourceType = typeof(AccountRes))]
        Admin = 3
    }

    public enum GameExpansion
    {
        NoExpansion = 0,
        TBC = 1,
        WotLK = 2,
        Cataclysm = 3,
        MoP = 4,
        WoD = 5,
        Legion = 6,
        BfA = 7
    }

    public class AccountManager
    {
        public const string FreedomBnetEmailConst = "@FREEDOM.COM";

        public static string BnetAccountCalculateShaHash(string username, string password)
        {
            string userEmail = (username + FreedomBnetEmailConst).ToUpper();
            string usernameEmailHash = FreedomShaHasher.Sha256HashHexdecimal(userEmail);
            return FreedomShaHasher.Sha256HashHexdecimal(usernameEmailHash + ":" + password.ToUpper(), true);
        }

        public static string GameAccountCalculateShaHash(string username, string password)
        {
            return FreedomShaHasher.Sha1HashHexdecimal(username.ToUpper() + ":" + password.ToUpper());
        }

        public static BnetAccount CreateBnetAccount(string username, string bnetAccSha256Pass)
        {
            BnetAccount bnetAcc = new BnetAccount()
            {
                UsernameEmail = username.ToUpper() + FreedomBnetEmailConst,
                Joined = DateTime.Now,
                LastLogin = DateTime.MinValue,
                ShaPassHash = bnetAccSha256Pass
            };

            return bnetAcc;
        }

        public static void UpdateBnetAccount(BnetAccount bnetAcc, string username, string bnetAccSha256Pass)
        {
            bnetAcc.UsernameEmail = username.ToUpper() + FreedomBnetEmailConst;
            bnetAcc.ShaPassHash = bnetAccSha256Pass;
        }

        public static GameAccount CreateGameAccount(int bnetAccId, string regEmail, string gameAccSha1Pass)
        {
            if (bnetAccId == 0)
            {
                throw new ArgumentException("BnetAccount ID must not be 0, when creating game account for it");
            }

            GameAccount gameAcc = new GameAccount()
            {
                Username = string.Format("{0}#{1}", bnetAccId.ToString(), "1"),
                ShaPassHash = gameAccSha1Pass,
                Email = regEmail,
                RegEmail = regEmail,
                Joined = DateTime.Now,
                Expansion = GameExpansion.BfA,
                BnetAccountId = bnetAccId,
                BnetAccountIndex = 1
            };

            return gameAcc;
        }

        public static void UpdateGameAccount(GameAccount gameAcc, string regEmail, string gameAccSha1Pass)
        {
            gameAcc.ShaPassHash = gameAccSha1Pass;
            gameAcc.Email = regEmail;
            gameAcc.RegEmail = regEmail;
        }

        public static void SetGameAccAccessLevel(int gameAccId, GMLevel gmlevel)
        {
            using (var db = new DbAuth())
            {
                GameAccountAccess gameAccAccess = db.GameAccountAccesses.Find(gameAccId);

                if (gameAccAccess != null)
                {
                    gameAccAccess.GMLevel = gmlevel;
                    db.Entry(gameAccAccess).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    gameAccAccess = new GameAccountAccess()
                    {
                        Id = gameAccId,
                        GMLevel = gmlevel
                    };

                    db.GameAccountAccesses.Add(gameAccAccess);
                }

                db.SaveChanges();
            }
        }

        public static GameAccountAccess GetGameAccAccess(int gameAccId)
        {            
            using (var db = new DbAuth())
            {
                GameAccountAccess gameAccAccess = db.GameAccountAccesses.Find(gameAccId);

                if (gameAccAccess != null)
                {
                    return gameAccAccess;
                }
                else
                {
                    gameAccAccess = new GameAccountAccess()
                    {
                        Id = gameAccId,
                        GMLevel = GMLevel.Player
                    };

                    db.GameAccountAccesses.Add(gameAccAccess);
                    db.SaveChanges();
                    return gameAccAccess;
                }
            }            
        }

        public static BnetAccount BnetAccGetByUsername(string username)
        {
            using (var db = new DbAuth())
            {
                return db.BnetAccounts
                    .AsNoTracking()
                    .Where(a => a.UsernameEmail == (username.ToUpper() + FreedomBnetEmailConst))
                    .FirstOrDefault();
            }
        }

        public static GameAccount GameAccGetByBnetKey(int bnetAccId)
        {
            using (var db = new DbAuth())
            {
                return db.GameAccounts
                    .AsNoTracking()
                    .Where(a => a.BnetAccountId == bnetAccId && a.BnetAccountIndex == 1)
                    .FirstOrDefault();
            }
        }

        public static string GeneratePasswordResetEmailBody(string username, string callbackUrl)
        {
            return string.Format(
                "Hello {0},<br /> <br />" +
                "A request for a new password was submitted for your WoW Freedom website account. " +
                "Simply follow the link to reset your password: <a href=\"{1}\">reset password</a> <br /> <br />" +
                "If the above URL does not work, please copy and paste the link below to your web browser: <br />" +
                "{2} <br /> <br />" +
                "Regards, <br /> WoW Freedom administration", 
                username, callbackUrl, callbackUrl);
        }

        public static List<GameAccount> GetGameAccountsList(int bnetAccId)
        {
            using (var db = new DbAuth())
            {
                return db.GameAccounts.Where(a => a.BnetAccountId == bnetAccId).ToList();
            }
        }
        
        public static int[] GetGameAccountIDs(int bnetAccId)
        {
            using (var db = new DbAuth())
            {
                return db.GameAccounts
                    .Where(a => a.BnetAccountId == bnetAccId)
                    .Select(a => a.Id)
                    .ToArray();
            }
        }
    }
}

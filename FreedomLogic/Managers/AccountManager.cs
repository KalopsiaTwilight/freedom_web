using FreedomLogic.DAL;
using FreedomLogic.Entities.Auth;
using FreedomLogic.Identity;
using FreedomLogic.Resources;
using FreedomLogic.TrinityCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

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
        Admin = 3,
        [Display(Name = "FieldGMLevelUnused", ResourceType = typeof(AccountRes))]
        Unused = 999,
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
        BfA = 7,
        Shadowlands = 8
    }

    public enum BanDuration
    {
        [Display(Name = "One Day")]
        OneDay,
        [Display(Name = "One Week")]
        OneWeek,
        [Display(Name = "Two Weeks")]
        TwoWeeks,
        [Display(Name = "One Month")]
        OneMonth,
        [Display(Name = "Permanent")]
        Permanent
    }

    public class AccountManager
    {

        private readonly DbAuth _authDb;

        public AccountManager(DbAuth authDb)
        {
            _authDb = authDb;
        }

        public const string FreedomBnetEmailConst = "@FREEDOM.COM";

        public string BnetAccountCalculateShaHash(string username, string password)
        {
            return FreedomShaHasher.CalculateBnetHash(username, password);
        }

        public BnetAccount CreateBnetAccount(string username, string bnetAccSha256Pass)
        {
            BnetAccount bnetAcc = new BnetAccount()
            {
                UsernameEmail = username.ToUpper() + FreedomBnetEmailConst,
                Joined = DateTime.Now,
                ShaPassHash = bnetAccSha256Pass
            };

            return bnetAcc;
        }

        public void UpdateBnetAccount(BnetAccount bnetAcc, string username, string bnetAccSha256Pass)
        {
            bnetAcc.UsernameEmail = username.ToUpper() + FreedomBnetEmailConst;
            bnetAcc.ShaPassHash = bnetAccSha256Pass;
        }

        public GameAccount CreateGameAccount(int bnetAccId, byte accountIndex, string regEmail, string password)
        {
            if (bnetAccId == 0)
            {
                throw new ArgumentException("BnetAccount ID must not be 0, when creating game account for it");
            }

            var salt = RandomNumberGenerator.GetBytes(32);
            var username = string.Format("{0}#{1}", bnetAccId.ToString(), accountIndex);
            GameAccount gameAcc = new GameAccount()
            {
                Username = username,
                Salt = salt,
                Verifier = SRP6.GetVerifier(username, password, salt),
                Email = regEmail,
                RegEmail = regEmail,
                Joined = DateTime.Now,
                Expansion = GameExpansion.Shadowlands,
                BnetAccountId = bnetAccId,
                BnetAccountIndex = accountIndex
            };

            return gameAcc;
        }

        public void UpdateGameAccount(GameAccount gameAcc, string regEmail, string password)
        {
            gameAcc.Email = regEmail;
            gameAcc.RegEmail = regEmail;
            if (!string.IsNullOrEmpty(password))
            {
                gameAcc.Salt = RandomNumberGenerator.GetBytes(32);
                gameAcc.Verifier = SRP6.GetVerifier(gameAcc.Username, password, gameAcc.Salt);
            }
        }

        public void SetGameAccAccessLevel(int bnetAccountId, GMLevel gmlevel)
        {
            var accounts = _authDb.GameAccounts.Where(x => x.BnetAccountId == bnetAccountId).ToList();
            foreach (var account in accounts)
            {
                var accountAccess = _authDb.GameAccountAccesses.Find(account.Id);
                if (accountAccess != null)
                {
                    accountAccess.GMLevel = gmlevel;
                    _authDb.Entry(accountAccess).State = EntityState.Modified;
                }
                else
                {
                    accountAccess = new GameAccountAccess()
                    {
                        GMLevel = gmlevel,
                        Id = account.Id,
                        RealmId = -1
                    };
                    _authDb.Add(accountAccess);
                }
            }
            _authDb.SaveChanges();
        }

        public GameAccountAccess GetGameAccAccess(int gameAccId)
        {
            GameAccountAccess gameAccAccess = _authDb.GameAccountAccesses.Find(gameAccId);

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

                _authDb.GameAccountAccesses.Add(gameAccAccess);
                _authDb.SaveChanges();
                return gameAccAccess;
            }
        }

        public BnetAccount BnetAccGetByUsername(string username)
        {
            return _authDb.BnetAccounts
                .AsNoTracking()
                .Where(a => a.UsernameEmail == (username.ToUpper() + FreedomBnetEmailConst))
                .FirstOrDefault();
        }

        public GameAccount GameAccGetByBnetKey(int bnetAccId)
        {
            return _authDb.GameAccounts
                .AsNoTracking()
                .Where(a => a.BnetAccountId == bnetAccId && a.BnetAccountIndex == 1)
                .FirstOrDefault();
        }

        public string GeneratePasswordResetEmailBody(string username, string callbackUrl)
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

        public List<GameAccount> GetGameAccountsList(int bnetAccId)
        {
            return _authDb.GameAccounts.Where(a => a.BnetAccountId == bnetAccId).ToList();
        }

        public int[] GetGameAccountIDs(int bnetAccId)
        {
            return _authDb.GameAccounts
                .Where(a => a.BnetAccountId == bnetAccId)
                .Select(a => a.Id)
                .ToArray();
        }


        public void BanGameAccounts(int bnetAccountId, BanDuration duration, string reason, string bannedBy)
        {
            var accounts = _authDb.GameAccounts.Where(x => x.BnetAccountId == bnetAccountId).ToList();
            foreach (var account in accounts)
            {
                var ban = _authDb.AccountBans.Find(account.Id);
                var isNew = ban == null;
                ban ??= new AccountBan()
                {
                    Id = account.Id,
                };
                ban.BanReason = reason;
                ban.Active = true;
                ban.BannedBy = bannedBy;
                ban.BanDate = DateTime.Now;
                switch(duration)
                {
                    case BanDuration.OneDay: ban.Unbandate = DateTime.Now.AddDays(1); break;
                    case BanDuration.OneWeek: ban.Unbandate = DateTime.Now.AddDays(7); break;
                    case BanDuration.OneMonth: ban.Unbandate = DateTime.Now.AddMonths(1); break;
                    case BanDuration.TwoWeeks: ban.Unbandate = DateTime.Now.AddDays(14); break;
                    case BanDuration.Permanent: ban.Unbandate = ban.BanDate; break;
                }
                if (isNew)
                {
                    _authDb.Add(ban);
                }
                else
                {
                    _authDb.Entry(ban).State = EntityState.Modified;
                }
            }
            _authDb.SaveChanges();
        }

        public void UnbanGameAccounts(int bnetAccountId)
        {
            var accounts = _authDb.GameAccounts.Where(x => x.BnetAccountId == bnetAccountId).ToList();
            foreach (var account in accounts)
            {
                var ban = _authDb.AccountBans.Find(account.Id);
                if (ban == null)
                {
                    continue;
                }
                ban.Active = false;
                ban.Unbandate = DateTime.Now;
                 _authDb.Entry(ban).State = EntityState.Modified;
            }
            _authDb.SaveChanges();
        }

    }
}

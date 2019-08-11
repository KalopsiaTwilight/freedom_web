using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    public class UserData
    {
        public UserData()
        {
        }

        public bool Load(int userId)
        {
            using (var dbFreedom = new DbFreedom())
            {
                WebUser = dbFreedom.Users.Find(userId);
            }

            if (WebUser == null)
                return false;

            BnetAccount = DbManager.GetByKey<BnetAccount, DbAuth>(WebUser.BnetAccountId);

            if (BnetAccount == null)
                return false;

            GameAccount = AccountManager.GameAccGetByBnetKey(BnetAccount.Id);

            if (GameAccount == null)
                return false;

            GameAccountAccess = AccountManager.GetGameAccAccess(GameAccount.Id);
            GameAccountCharacters = CharacterManager.GetAccountCharacters(GameAccount.Id);
            //BNetAccountCharacters = CharacterManager.GetBNetAccountCharacters(BnetAccount.Id);

            return true;
        }

        public User WebUser { get; set; }

        public List<Character> GameAccountCharacters { get; set; }

        //public List<Character> BNetAccountCharacters { get; set; }

        public BnetAccount BnetAccount { get; set; }

        public GameAccount GameAccount { get; set; }

        public GameAccountAccess GameAccountAccess { get; set; }
    }
}

using FreedomLogic.DAL;
using FreedomLogic.Entities.Auth;
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

        public User WebUser { get; set; }

        //public List<Character> BNetAccountCharacters { get; set; }

        public BnetAccount BnetAccount { get; set; }

        public GameAccount GameAccount { get; set; }

        public GameAccountAccess GameAccountAccess { get; set; }
    }
}

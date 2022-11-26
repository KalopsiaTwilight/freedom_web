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
    public class CharData
    {
        public string ZoneName { get; set; }

        public string MapName { get; set; }

        public RaceInfo RaceData { get; set; }

        public ClassInfo ClassData { get; set; }

        public User WebUser { get; set; }

        public List<Character> GameAccountCharacters { get; set; }

        public BnetAccount BnetAccount { get; set; }

        public GameAccount GameAccount { get; set; }

        public GameAccountAccess GameAccountAccess { get; set; }

        public Character ExtraFlags { get; set; }
    }
}

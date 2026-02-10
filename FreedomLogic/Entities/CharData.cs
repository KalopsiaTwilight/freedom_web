using FreedomLogic.Entities.Auth;
using FreedomLogic.Entities.Characters;
using FreedomLogic.Entities.Freedom;
using FreedomLogic.Identity;
using System.Collections.Generic;

namespace FreedomLogic.Entities
{
    public class CharData
    {
        public string ZoneName { get; set; }

        public string MapName { get; set; }

        public string Phase { get; set; }

        public RaceInfo RaceData { get; set; }

        public ClassInfo ClassData { get; set; }

        public User WebUser { get; set; }

        public List<Character> GameAccountCharacters { get; set; }

        public BnetAccount BnetAccount { get; set; }

        public GameAccount GameAccount { get; set; }

        public GameAccountAccess GameAccountAccess { get; set; }
    }
}

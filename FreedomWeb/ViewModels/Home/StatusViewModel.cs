using FreedomLogic.Managers;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Home
{
    public class StatusViewModel
    {
        public StatusViewModel()
        {
            Characters = new List<StatusCharListItem>();

            foreach (var character in CharacterManager.GetOnlineCharacters())
            {
                var raceIconPath = character.Gender == CharGender.Male ? character.CharData.RaceData.IconMalePath : character.CharData.RaceData.IconFemalePath;
                Characters.Add(new StatusCharListItem()
                {
                    UserId = character.CharData.WebUser.Id,
                    FactionIconPath = character.CharData.RaceData.IconFactionPath,
                    Name = character.Name,
                    Owner = character.CharData.WebUser.DisplayName,
                    OwnerUsername = character.CharData.WebUser.UserName,
                    Race = character.CharData.RaceData.Name,
                    RaceIconPath = raceIconPath,
                    Class = character.CharData.ClassData.Name,
                    ClassIconPath = character.CharData.ClassData.IconPath,
                    Gender = Enum.GetName(character.Gender.GetType(), character.Gender),
                    MapName = character.CharData.MapName,
                    ZoneName = character.CharData.ZoneName
                });
            }
        }

        public List<StatusCharListItem> Characters { get; set; }
    }
}
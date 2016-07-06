using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomUtils.MvcUtils;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Admin
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            UserList = new List<UserListItem>();

            foreach (var user in DbManager.SelectAll<User, DbFreedom>(include: "FreedomRoles"))
            {
                var gmLevel = user.UserData.GameAccountAccess.GMLevel;
                UserList.Add(new UserListItem()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.RegEmail,
                    Roles = string.Join(", ", user.FreedomRoles.Select(r => r.Name)),
                    GameAccess = gmLevel.DisplayName(),
                });
            }
        }

        public List<UserListItem> UserList { get; set; }
    }
}
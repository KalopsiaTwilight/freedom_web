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
    public class AdminCPViewModel
    {
        public AdminCPViewModel()
        {
            AdminList = new List<AdminListItem>();

            foreach(var admin in DbManager.SelectAll<User, DbFreedom>(include: "FreedomRoles")
                .Where(u => u.FreedomRoles.Any(r => r.Name == FreedomRole.RoleAdmin)))
            {
                var gmLevel = admin.UserData.GameAccountAccess.GMLevel;
                AdminList.Add(new AdminListItem()
                {
                    UserId = admin.Id,
                    Username = admin.UserName,
                    DisplayName = admin.DisplayName,
                    Email = admin.RegEmail,
                    Roles = string.Join(", ", admin.FreedomRoles.Select(r => r.Name)),
                    GameAccess = gmLevel.DisplayName(),
                });
            }
        }

        public List<AdminListItem> AdminList { get; set; }
    }
}
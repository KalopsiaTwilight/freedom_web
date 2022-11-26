using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomUtils.MvcUtils;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.ViewModels.Admin
{
    public class UserListViewModel
    {
        public List<UserListItem> UserList { get; set; }
    }
}
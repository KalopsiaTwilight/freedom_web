using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.Models
{
    public class UserListItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string RegEmail { get; set; }
        public string Roles { get; set; }
        public string GameAccess { get; set; }
    }
}
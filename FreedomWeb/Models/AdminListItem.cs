using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.Models
{
    public class AdminListItem
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string GameAccess { get; set; }
    }
}
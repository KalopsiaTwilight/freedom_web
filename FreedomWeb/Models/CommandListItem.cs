using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreedomWeb.Models
{
    public class CommandListItem
    {
        public string Command { get; set; }
        public string Syntax { get; set; }
        public string Description { get; set; }
        public string GameAccountAccess { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.DTFilters
{
    public class CommandListFilter
    {
        public CommandListFilter()
        {
            Command = "";
            Syntax = "";
            Description = "";
            GMLevelDisplay = "";
        }

        public string Command { get; set; }
        public string Syntax { get; set; }
        public string Description { get; set; }
        public string GMLevelDisplay { get; set; }
    }
}

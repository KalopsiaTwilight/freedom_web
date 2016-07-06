using FreedomLogic.Entities;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Server
{
    public class CommandListViewModel
    {
        public CommandListViewModel()
        {
            CommandList = new List<FreedomCommand>();
        }

        public List<FreedomCommand> CommandList { get; set; }
    }
}
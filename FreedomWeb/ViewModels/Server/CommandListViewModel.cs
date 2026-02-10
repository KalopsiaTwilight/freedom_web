using FreedomLogic.Entities.Freedom;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
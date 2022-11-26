using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Admin
{
    public enum EnumServerAppStatus
    {
        Offline,
        Loading,
        Online
    }

    public class ServerControlViewModel
    {
        public ServerControlViewModel()
        {
            WorldServerStatus = EnumServerAppStatus.Offline;
            BnetServerStatus = EnumServerAppStatus.Offline;
        }

        [Display(Name = "FieldWorldServerPath", ResourceType = typeof(ServerRes))]
        public string WorldServerPath { get; set; }

        [Display(Name = "FieldWorldServerPid", ResourceType = typeof(ServerRes))]
        public int WorldServerPid { get; set; }

        [Display(Name = "FieldWorldServerStatus", ResourceType = typeof(ServerRes))]
        public EnumServerAppStatus WorldServerStatus { get; set; }

        [Display(Name = "FieldBnetServerPath", ResourceType = typeof(ServerRes))]
        public string BnetServerPath { get; set; }

        [Display(Name = "FieldBnetServerPid", ResourceType = typeof(ServerRes))]
        public int BnetServerPid { get; set; }

        [Display(Name = "FieldBnetServerStatus", ResourceType = typeof(ServerRes))]
        public EnumServerAppStatus BnetServerStatus { get; set; }

        [Display(Name = "FieldServerDirectoryPath", ResourceType = typeof(ServerRes))]
        public string ServerDirectoryPath { get; set; }
    }
}
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Home
{
    public enum EnumFreedomGameserverStatus
    {
        [Display(Name = "FieldServerStatusOffline", ResourceType = typeof(ServerRes))]
        Offline, // Both down

        [Display(Name = "FieldServerStatusOnline", ResourceType = typeof(ServerRes))]
        Online, // Both up

        [Display(Name = "FieldServerStatusLoginDown", ResourceType = typeof(ServerRes))]
        LoginDown, // Only BnetServer down

        [Display(Name = "FieldServerStatusWorldDown", ResourceType = typeof(ServerRes))]
        WorldDown, // Only WorldServer down

        [Display(Name = "FieldServerStatusWorldLoading", ResourceType = typeof(ServerRes))]
        WorldLoading // Worldserver is starting up/loading
    }

    public class StatusViewModel
    {
        public StatusViewModel()
        {
            bool bnetServerRunning = ServerManager.Control.IsBnetServerRunning();
            bool worldServerRunning = ServerManager.Control.IsWorldServerRunning();
            bool worldServerOnline = ServerManager.Control.IsWorldServerOnline();

            if (!bnetServerRunning && !worldServerRunning)
            {
                Status = EnumFreedomGameserverStatus.Offline;
            }
            else if (worldServerRunning && !worldServerOnline)
            {
                Status = EnumFreedomGameserverStatus.WorldLoading;
            }
            else if (!worldServerRunning && bnetServerRunning)
            {
                Status = EnumFreedomGameserverStatus.WorldDown;
            }
            else if (worldServerRunning && !bnetServerRunning)
            {
                Status = EnumFreedomGameserverStatus.LoginDown;
            }
            else
            {
                Status = EnumFreedomGameserverStatus.Online;
            }
        }

        public EnumFreedomGameserverStatus Status { get; set; }
    }
}
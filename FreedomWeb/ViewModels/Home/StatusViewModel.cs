using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        }

        public EnumFreedomGameserverStatus Status { get; set; }
    }
}
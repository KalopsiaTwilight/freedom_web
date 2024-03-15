using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Admin
{
    public enum DboServerAppStatus
    {
        Offline,
        Online
    }

    public class DboServerControlViewModel
    {
        public DboServerControlViewModel()
        {
            MasterServerStatus = DboServerAppStatus.Offline;
            QueryServerStatus = DboServerAppStatus.Offline;
            CharServerStatus = DboServerAppStatus.Offline;
            ChatServerStatus = DboServerAppStatus.Offline;
            GameServerStatus = DboServerAppStatus.Offline;
            AuthServerStatus = DboServerAppStatus.Offline;
        }

        public const string MasterServerId = "Master";
        public const string QueryServerId = "Query";
        public const string CharServerId = "Char";
        public const string ChatServerId = "Chat";
        public const string GameServerId = "Game";
        public const string AuthServerId = "Auth";

        [Display(Name = "Server Directory Path")]
        public string ServerDirectoryPath { get; set; }

        [Display(Name = "Master Server Path")]
        public string MasterServerPath { get; set; }
        [Display(Name = "Master Server Process Id")]
        public int MasterServerPid { get; set; }
        [Display(Name = "Master Server Status")]
        public DboServerAppStatus MasterServerStatus { get; set; }

        [Display(Name = "Query Server Path")]
        public string QueryServerPath { get; set; }
        [Display(Name = "Query Server Process Id")]
        public int QueryServerPid { get; set; }
        [Display(Name = "Query Server Status")]
        public DboServerAppStatus QueryServerStatus { get; set; }

        [Display(Name = "Character Server Path")]
        public string CharServerPath { get; set; }
        [Display(Name = "Character Server Process Id")]
        public int CharServerPid { get; set; }
        [Display(Name = "Character Server Status")]
        public DboServerAppStatus CharServerStatus { get; set; }

        [Display(Name = "Chat Server Path")]
        public string ChatServerPath { get; set; }
        [Display(Name = "Chat Server Process Id")]
        public int ChatServerPid { get; set; }
        [Display(Name = "Chat Server Status")]
        public DboServerAppStatus ChatServerStatus { get; set; }

        [Display(Name = "Game Server Path")]
        public string GameServerPath { get; set; }
        [Display(Name = "Game Server Process Id")]
        public int GameServerPid { get; set; }
        [Display(Name = "Game Server Status")]
        public DboServerAppStatus GameServerStatus { get; set; }

        [Display(Name = "Authentication Server Path")]
        public string AuthServerPath { get; set; }
        [Display(Name = "Authentication Server Process Id")]
        public int AuthServerPid { get; set; }
        [Display(Name = "Authentication Server Status")]
        public DboServerAppStatus AuthServerStatus { get; set; }
    }
}
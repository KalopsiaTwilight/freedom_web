using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    public static class SettingsManager
    {
        public static string GetServerDir()
        {
            return ConfigurationManager.AppSettings["serverDir"];
        }

        public static string GetWorldServerPidFilename()
        {
            return ConfigurationManager.AppSettings["worldserverPidFilename"];
        }

        public static string GetBnetServerPidFilename()
        {
            return ConfigurationManager.AppSettings["bnetserverPidFilename"];
        }

        public static string GetWorldServerPath()
        {
            return Path.Combine(GetServerDir(), "worldserver.exe");
        }

        public static string GetBnetServerPath()
        {
            return Path.Combine(GetServerDir(), "bnetserver.exe");
        }

        public static string GetWorldServerFilename()
        {
            return "worldserver.exe";
        }

        public static string GetBnetServerFilename()
        {
            return "bnetserver.exe";
        }
       
        public static int GetRealmId()
        {
            return int.Parse(ConfigurationManager.AppSettings["realmId"]);
        }
    }
}

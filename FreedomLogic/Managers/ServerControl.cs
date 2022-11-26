using FreedomLogic.DAL;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using FreedomUtils.Win32APIUtils;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.Managers
{
    public class ServerControl
    {
        private const byte ServerOfflineFlag = 0x2;
        private readonly DbAuth _authDb;

        public ServerControl(DbAuth authDb)
        {
            _authDb = authDb;
        }

        public int GetWorldServerPid()
        {
            string pidPath = Path.Combine(SettingsManager.GetServerDir(), SettingsManager.GetWorldServerPidFilename());
            int pid = 0;

            if (!File.Exists(pidPath))
                return 0;

            try
            {
                string input = File.ReadAllText(pidPath);
                int.TryParse(input, out pid);
            }
            catch (IOException) // TODO: Log exception
            {
                return 0;
            }

            return pid;
        }

        public int GetBnetServerPid()
        {
            string pidPath = Path.Combine(SettingsManager.GetServerDir(), SettingsManager.GetBnetServerPidFilename());
            int pid = 0;

            if (!File.Exists(pidPath))
                return 0;

            try
            {
                string input = File.ReadAllText(pidPath);
                int.TryParse(input, out pid);
            }
            catch (IOException) // TODO: Log exception
            {
                return 0;
            }

            return pid;
        }

        public bool IsWorldServerRunning()
        {
            int pid = GetWorldServerPid();
            var process = Process.GetProcesses().Where(p => p.Id == pid).FirstOrDefault();

            if (process == null)
            {
                return false;
            }

            if (process.ProcessName != "worldserver")
            {
                return false;
            }

            return true;
        }

        public bool IsBnetServerRunning()
        {
            int pid = GetBnetServerPid();
            var process = Process.GetProcesses().Where(p => p.Id == pid).FirstOrDefault();

            if (process == null)
            {
                return false;
            }

            if (process.ProcessName != "bnetserver")
            {
                return false;
            }

            return true;
        }

        public bool IsWorldServerOnline()
        {
            if (!IsWorldServerRunning())
                return false;

            int realmId = SettingsManager.GetRealmId();

            var realm = _authDb.Realmlists.Where(r => r.Id == realmId).FirstOrDefault();

            if (realm == null)
                return false;

            if ((realm.Flags & ServerOfflineFlag) != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool StopBnetServer(out string error)
        {
            if (IsBnetServerRunning())
            {
                try
                {
                    // shut down the bnet server
                    int pid = GetBnetServerPid();
                    var process = Process.GetProcesses().Where(p => p.Id == pid).FirstOrDefault();

                    if (process != null)
                    {
                        process.Kill();
                    }
                }
                catch (Exception e)
                {
                    error = string.Format("[{0} @ {1}]: {2}", e.GetType().FullName, e.TargetSite.ReflectedType.Name + "." + e.TargetSite.Name, e.Message);
                    return false;
                }
            }

            error = "";
            return true;
        }

        public bool StartBnetServer(out string error)
        {
            if (IsBnetServerRunning())
            {
                error = "BnetServer is already running";
                return false;
            }

            try
            {
                string exePath = SettingsManager.GetBnetServerPath();
                string workingDir = SettingsManager.GetServerDir();
                int sessionId = SettingsManager.GetProcessStartSessionId();
                ProcessExtensions.StartProcessForSessionId(sessionId, null, exePath, workingDir, true);
            }
            catch (Exception e)
            {
                error = string.Format("[{0} @ {1}]: {2}", e.GetType().FullName, e.TargetSite.ReflectedType.Name + "." + e.TargetSite.Name, e.Message);
                return false;
            }

            error = "";
            return true;
        }

        public bool StopWorldServer(out string error)
        {
            if (IsWorldServerRunning())
            {
                try
                {
                    // shut down the world server
                    int pid = GetWorldServerPid();
                    var process = Process.GetProcesses().Where(p => p.Id == pid).FirstOrDefault();

                    if (process != null)
                    {
                        process.Kill();
                    }

                    int realmId = SettingsManager.GetRealmId();

                    var realm = _authDb.Realmlists.Find(realmId);
                    realm.Flags = (byte)(realm.Flags | ServerOfflineFlag);
                    _authDb.Entry(realm).State = EntityState.Modified;
                    _authDb.SaveChanges();
                }
                catch (Exception e)
                {
                    error = string.Format("[{0} @ {1}]: {2}", e.GetType().FullName, e.TargetSite.ReflectedType.Name + "." + e.TargetSite.Name, e.Message);
                    return false;
                }

            }

            error = "";
            return true;
        }

        public bool StartWorldServer(out string error)
        {
            if (IsWorldServerRunning())
            {
                error = "WorldServer is already running";
                return false;
            }

            try
            {
                int realmId = SettingsManager.GetRealmId();

                var realm = _authDb.Realmlists.Find(realmId);
                realm.Flags = (byte)(realm.Flags | ServerOfflineFlag);
                _authDb.Entry(realm).State = EntityState.Modified;
                _authDb.SaveChanges();

                string exePath = SettingsManager.GetWorldServerPath();
                string workingDir = SettingsManager.GetServerDir();
                int sessionId = SettingsManager.GetProcessStartSessionId();
                ProcessExtensions.StartProcessForSessionId(sessionId, null, exePath, workingDir, true);
            }
            catch (Exception e)
            {
                error = string.Format("[{0} @ {1}]: {2}", e.GetType().FullName, e.TargetSite.ReflectedType.Name + "." + e.TargetSite.Name, e.Message);
                return false;
            }

            error = "";
            return true;
        }
    }
}

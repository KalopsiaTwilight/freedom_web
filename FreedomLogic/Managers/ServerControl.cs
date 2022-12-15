using FreedomLogic.DAL;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using FreedomLogic.Infrastructure;

namespace FreedomLogic.Managers
{
    public class ServerControl
    {
        private const byte ServerOfflineFlag = 0x2;
        private readonly DbAuth _authDb;
        private readonly DbWorld _worldDb;
        private readonly AppConfiguration _appConfig;

        public ServerControl(DbAuth authDb, DbWorld worldDb, AppConfiguration config)
        {
            _authDb = authDb;
            _appConfig = config;
            _worldDb = worldDb;
        }

        public int GetWorldServerPid()
        {
            string pidPath = Path.Combine(_appConfig.TrinityCore.ServerDir, _appConfig.TrinityCore.WorldServerPidFilename);
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
            string pidPath = Path.Combine(_appConfig.TrinityCore.ServerDir, _appConfig.TrinityCore.BnetServerPidFilename);
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

            int realmId = _appConfig.TrinityCore.RealmId;

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
                        var stopped = process.CloseMainWindow();
                        if (!stopped)
                        {
                            process.Kill();
                        }
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
                string exePath = Path.Combine(_appConfig.TrinityCore.ServerDir, "bnetserver.exe");
                string workingDir = _appConfig.TrinityCore.ServerDir;
                int sessionId = _appConfig.TrinityCore.ProcessStartSessionId;
                //ProcessExtensions.StartProcessForSessionId(sessionId, null, exePath, workingDir, true);
                //ProcessExtensions.StartProcessAsCurrentUser(exePath, null, null, true);
                ProcessStartInfo psInfo = new()
                {
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    FileName = exePath,
                    WorkingDirectory =workingDir,
                    CreateNoWindow=false
                };
                Process.Start(psInfo);
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
                        var stopped = process.CloseMainWindow();
                        if (!stopped)
                        {
                            process.Kill();
                        }
                    }

                    int realmId = _appConfig.TrinityCore.RealmId;

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
                int realmId = _appConfig.TrinityCore.RealmId;

                var realm = _authDb.Realmlists.Find(realmId);
                realm.Flags = (byte)(realm.Flags | ServerOfflineFlag);
                _authDb.Entry(realm).State = EntityState.Modified;
                _authDb.SaveChanges();

                string exePath = Path.Combine(_appConfig.TrinityCore.ServerDir, "worldserver.exe");
                string workingDir = _appConfig.TrinityCore.ServerDir;
                int sessionId = _appConfig.TrinityCore.ProcessStartSessionId;
                //ProcessExtensions.StartProcessForSessionId(sessionId, null, exePath, workingDir, true);
                //ProcessExtensions.StartProcessAsCurrentUser(exePath, null, null, true);
                ProcessStartInfo psInfo = new()
                {
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    FileName = exePath,
                    WorkingDirectory = workingDir
                };
                Process.Start(psInfo);
            }
            catch (Exception e)
            {
                error = string.Format("[{0} @ {1}]: {2}", e.GetType().FullName, e.TargetSite.ReflectedType.Name + "." + e.TargetSite.Name, e.Message);
                return false;
            }

            error = "";
            return true;
        }

        public bool RunFixQuery()
        {
            _worldDb.Database.ExecuteSqlInterpolated(@$"
UPDATE `creature` SET `spawnDifficulties` = '0' WHERE `guid` >= 500000 AND `spawnDifficulties` NOT LIKE '0';
UPDATE `gameobject` SET `spawnDifficulties` = '0' WHERE `guid` >= 500000 AND `spawnDifficulties` NOT LIKE '0';");
            return true;
        }
    }
}

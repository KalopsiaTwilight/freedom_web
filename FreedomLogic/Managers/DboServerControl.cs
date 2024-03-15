using FreedomLogic.DAL;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using FreedomUtils.Win32APIUtils;
using Microsoft.EntityFrameworkCore;
using FreedomLogic.Infrastructure;

namespace FreedomLogic.Managers
{
    public class DboServerControl
    {
        private readonly AppConfiguration _appConfig;

        public DboServerControl(AppConfiguration config)
        {
            _appConfig = config;
        }

        public int GetMasterServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.MasterPidFileName));
        }
        public int GetQueryServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.QueryPidFileName));
        }
        public int GetCharServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.CharPidFileName));
        }
        public int GetChatServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.ChatPidFileName));
        }
        public int GetGameServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.GamePidFileName));
        }
        public int GetAuthServerPid()
        {
            return GetPid(Path.Combine(_appConfig.Dbo.RootPath, _appConfig.Dbo.AuthPidFileName));
        }

        public int GetPid(string filePath)
        {
            int pid = 0;

            if (!File.Exists(filePath))
                return 0;

            try
            {
                string input = File.ReadAllText(filePath);
                int.TryParse(input, out pid);
            }
            catch (IOException) // TODO: Log exception
            {
                return 0;
            }

            return pid;
        }


        public bool IsMasterServerRunning()
        {
            return IsProcessRunning(GetMasterServerPid(), "MasterServer");
        }
        public bool IsQueryServerRunning()
        {
            return IsProcessRunning(GetQueryServerPid(), "QueryServer");
        }
        public bool IsCharServerRunning()
        {
            return IsProcessRunning(GetCharServerPid(), "CharServer");
        }
        public bool IsChatServerRunning()
        {
            return IsProcessRunning(GetChatServerPid(), "ChatServer");
        }
        public bool IsGameServerRunning()
        {
            return IsProcessRunning(GetGameServerPid(), "GameServer");
        }
        public bool IsAuthServerRunning()
        {
            return IsProcessRunning(GetAuthServerPid(), "AuthServer");
        }

        public bool IsProcessRunning(int pid, string processName)
        {
            var process = Process.GetProcesses().Where(p => p.Id == pid).FirstOrDefault();

            if (process == null)
            {
                return false;
            }

            if (process.ProcessName != processName)
            {
                return false;
            }

            return true;
        }

        public bool StopMasterServer(out string error)
        {
            return StopProcess(GetMasterServerPid(), "MasterServer", out error);
        }
        public bool StopQueryServer(out string error)
        {
            return StopProcess(GetQueryServerPid(), "QueryServer", out error);
        }
        public bool StopCharServer(out string error)
        {
            return StopProcess(GetCharServerPid(), "CharServer", out error);
        }
        public bool StopChatServer(out string error)
        {
            return StopProcess(GetChatServerPid(), "ChatServer", out error);
        }
        public bool StopGameServer(out string error)
        {
            return StopProcess(GetGameServerPid(), "GameServer", out error);
        }
        public bool StopAuthServer(out string error)
        {
            return StopProcess(GetAuthServerPid(), "AuthServer", out error);
        }
        public bool StopProcess(int pid, string processName, out string error)
        {
            if (IsProcessRunning(pid, processName))
            {
                try
                {
                    // shut down the bnet server
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

        public bool StartMasterServer(out string error)
        {
            return StartProcess(GetMasterServerPid(), "MasterServer", Path.Combine(_appConfig.Dbo.RootPath, "start_master_server.bat"), out error);
        }
        public bool StartQueryServer(out string error)
        {
            return StartProcess(GetQueryServerPid(), "QueryServer", Path.Combine(_appConfig.Dbo.RootPath, "start_query_server.bat"), out error);
        }
        public bool StartCharServer(out string error)
        {
            return StartProcess(GetCharServerPid(), "CharServer", Path.Combine(_appConfig.Dbo.RootPath, "start_char_server_0.bat"), out error);
        }
        public bool StartChatServer(out string error)
        {
            return StartProcess(GetChatServerPid(), "ChatServer", Path.Combine(_appConfig.Dbo.RootPath, "start_chat_server.bat"), out error);
        }
        public bool StartGameServer(out string error)
        {
            return StartProcess(GetGameServerPid(), "GameServer", Path.Combine(_appConfig.Dbo.RootPath, "start_channel_0.bat"), out error);
        }
        public bool StartAuthServer(out string error)
        {
            return StartProcess(GetAuthServerPid(), "AuthServer", Path.Combine(_appConfig.Dbo.RootPath, "start_auth_server.bat"), out error);
        }

        public bool StartProcess(int pid, string processName, string exePath, out string error)
        {
            if (IsProcessRunning(pid, processName))
            {
                error = processName + " is already running";
                return false;
            }

            try
            {
                string workingDir = _appConfig.Dbo.RootPath;
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
    }
}

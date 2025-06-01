using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Admin;
using FreedomUtils.MvcUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreedomLogic.Infrastructure;
using System.IO;
using Microsoft.EntityFrameworkCore.Internal;
using FreedomLogic.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Text;
using System.Xml;
using FreedomUtils.DataTables;
using FreedomWeb.ViewModels.Item;
using FreedomLogic.Entities;

namespace FreedomWeb.Controllers
{
    [Authorize(Roles = FreedomRole.RoleAdmin)]
    public class AdminController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly DbFreedom _freedomDb;
        private readonly DbAuth _authDb;
        private readonly DbDboAcc _dbDboAcc;
        private readonly AccountManager _accountManager;
        private readonly ServerControl _serverControl;
        private readonly DboServerControl _dboServerControl;
        private readonly AppConfiguration _appConfig;
        private readonly ExtraDataLoader _dataLoader;
        private readonly HttpClient _httpClient;
        private readonly DbDboChar _dbDboChar;

        public AdminController(UserManager<User> userManager, DbFreedom freedomDb, AccountManager accountManager,
            ServerControl serverControl, AppConfiguration appConfig, ExtraDataLoader dataLoader, HttpClient httpClient, 
            DboServerControl dboServerControl, DbDboAcc dbDboAcc, DbDboChar dbDboChar, DbAuth authDb)
        {
            _userManager = userManager;
            _freedomDb = freedomDb;
            _accountManager = accountManager;
            _serverControl = serverControl;
            _appConfig = appConfig;
            _dataLoader = dataLoader;
            _httpClient = httpClient;
            _dboServerControl = dboServerControl;
            _dbDboAcc = dbDboAcc;
            _dbDboChar = dbDboChar;
            _authDb = authDb;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AdminCPViewModel
            {
                AdminList = new List<AdminListItem>()
            };
            var adminUsers = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(u => u.FreedomRoles.Any(r => r.Name == FreedomRole.RoleAdmin))
                .ToList();
            foreach (var admin in adminUsers)
            {
                _dataLoader.LoadExtraUserData(admin);
                var gmLevel = admin.UserData.GameAccountAccess.GMLevel;
                model.AdminList.Add(new AdminListItem()
                {
                    UserId = admin.Id,
                    Username = admin.UserName,
                    DisplayName = admin.DisplayName,
                    Email = admin.RegEmail,
                    Roles = string.Join(", ", admin.FreedomRoles.Select(r => r.Name)),
                    GameAccess = gmLevel.DisplayName(),
                });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UserListInfo(DTParameterModel parameters)
        {
            var search = parameters.Search.Value ?? "";

            var users = _freedomDb.Users
               .ToList();

            var dataQuery = _freedomDb.Users
                .Where(x => x.UserName.ToUpper().Contains(search.ToUpper())
                         || x.DisplayName.ToUpper().Contains(search)
                         || x.RegEmail.ToUpper().Contains(search)
                 )
                .Include(x => x.FreedomRoles);
            var dbData = await dataQuery
                .ApplyDataTableParameters(parameters)
                .ToListAsync();

            var data = dbData
                            .Select(user =>
                            {
                                _dataLoader.LoadExtraUserData(user);
                                var gmLevel = user.UserData?.GameAccountAccess?.GMLevel ?? GMLevel.Unused;
                                return new UserListItem
                                {
                                    Id = user.Id,
                                    UserName = user.UserName,
                                    DisplayName = user.DisplayName,
                                    RegEmail = user.RegEmail,
                                    Roles = string.Join(", ", user.FreedomRoles.Select(r => r.Name)),
                                    GameAccess = gmLevel.DisplayName(),
                                };
                            });

            var total = await _freedomDb.ItemBonusIdInfos.CountAsync();

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = dataQuery.Count(),
                data = data
            });
        }

        [HttpGet]
        public async Task<ActionResult> SetGameAccess(int? id)
        {
            var user = await _userManager.FindByIdAsync(id?.ToString());
            _dataLoader.LoadExtraUserData(user);
            if (user == null)
            {
                SetAlertMsg($"Account with id: {id?.ToString()} could not be found.", AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var dboAcc = await _dbDboAcc.Accounts.FirstOrDefaultAsync(x => x.Email == user.UserName + "@FREEDOM.COM");

            var model = new SetGameAccessViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                BnetAccountId = user.UserData.BnetAccount.Id,
                AccountAccess = user.UserData.GameAccountAccess.GMLevel,
                DboAccountAccess = dboAcc?.AdminLevel2 ?? DboAccountLevel.NoAccess
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SetGameAccess(SetGameAccessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _accountManager.SetGameAccAccessLevel(model.BnetAccountId, model.AccountAccess);

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            var dboAcc = await _dbDboAcc.Accounts.FirstOrDefaultAsync(x => x.Email == user.UserName + "@FREEDOM.COM");

            if (dboAcc != null)
            {
                if (model.DboAccountAccess > DboAccountLevel.NoAccess)
                {
                    dboAcc.AccountStatus = "active";
                } else
                {
                    dboAcc.AccountStatus = "block";
                }
                dboAcc.AdminLevel = model.DboAccountAccess;
                dboAcc.AdminLevel2 = model.DboAccountAccess;
                _dbDboAcc.Update(dboAcc);

                var characters = await _dbDboChar.Characters.Where(x => x.AccountId == dboAcc.Id).ToListAsync();
                foreach(var character in characters)
                {
                    character.AccountLevel = model.DboAccountAccess;
                    _dbDboChar.Update(character);
                }

                await _dbDboChar.SaveChangesAsync();
                await _dbDboAcc.SaveChangesAsync();
            }

            SetAlertMsg(AlertRes.AlertSuccessSetGameAccess, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Admin");
        }

        #region WoW Server Control
        [HttpGet]
        public ActionResult ServerControl()
        {
            var model = new ServerControlViewModel();
            LoadServerControlDataViewModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult ServerControlData()
        {
            var model = new ServerControlViewModel();
            LoadServerControlDataViewModel(model);

            return PartialView("_ServerControlData", model);
        }

        private void LoadServerControlDataViewModel(ServerControlViewModel model)
        {
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool serverOnline = _serverControl.IsWorldServerOnline();

            model.ServerDirectoryPath = _appConfig.TrinityCore.ServerDir;
            model.WorldServerPath = Path.Combine(_appConfig.TrinityCore.ServerDir, "worldserver.exe");
            model.BnetServerPath = Path.Combine(_appConfig.TrinityCore.ServerDir, "bnetserver.exe");
            model.WorldServerPid = worldServerRunning ? _serverControl.GetWorldServerPid() : 0;
            model.BnetServerPid = bnetServerRunning ? _serverControl.GetBnetServerPid() : 0;
            model.WorldServerStatus = worldServerRunning ? (serverOnline ? EnumServerAppStatus.Online : EnumServerAppStatus.Loading) : EnumServerAppStatus.Offline;
            model.BnetServerStatus = bnetServerRunning ? EnumServerAppStatus.Online : EnumServerAppStatus.Offline;
        }

        [HttpPost]
        public JsonResult ServerControlActions()
        {
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();

            return Json(new { worldServerRunning, bnetServerRunning });
        }

        [HttpPost]
        public JsonResult ServerControlStopWorldServer()
        {
            string error;
            bool stopSuccessful = _serverControl.StopWorldServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStopBnetServer()
        {
            string error;
            bool stopSuccessful = _serverControl.StopBnetServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartWorldServer()
        {
            string error;
            bool startSuccessful = _serverControl.StartWorldServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartBnetServer()
        {
            string error;
            bool startSuccessful = _serverControl.StartBnetServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }

        [HttpPost]
        public async Task<JsonResult> ServerControlSendRemoteCommand(string commandText)
        {
            try
            {
                string soapUri = $"http://{_appConfig.TrinityCore.SoapAddress}:{_appConfig.TrinityCore.SoapPort}/";
                var basicAuthHeaderValue = $"{_appConfig.TrinityCore.SoapUser}:{_appConfig.TrinityCore.SoapPassword}";
                basicAuthHeaderValue = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(basicAuthHeaderValue));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
                var result = await _httpClient.PostAsync(soapUri, new StringContent($@"<?xml version=""1.0"" encoding=""utf-8""?>
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:TC"" xmlns:xsd=""http://www.w3.org/1999/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
    <SOAP-ENV:Body>
        <ns1:executeCommand>
            <command>{commandText}</command>
        </ns1:executeCommand>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>", new MediaTypeHeaderValue("text/xml")));
                result.EnsureSuccessStatusCode();
                var resultBody = await result.Content.ReadAsStringAsync();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultBody);
                var resultNode = xml.SelectNodes("//result")?.Item(0);
                return Json(new { success = true, message = resultNode.InnerText });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message.ToString() });
            }
        }
        #endregion

        [HttpGet]
        public ActionResult DbogServerControl()
        {
            var model = new DboServerControlViewModel();
            LoadDboServerControlDataViewModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult DboServerControlData()
        {
            var model = new DboServerControlViewModel();
            LoadDboServerControlDataViewModel(model);

            return PartialView("_DboServerControlData", model);
        }


        private void LoadDboServerControlDataViewModel(DboServerControlViewModel model)
        {
            bool masterServerRunning = _dboServerControl.IsMasterServerRunning();
            bool queryServerRunning = _dboServerControl.IsQueryServerRunning();
            bool charServerRunning = _dboServerControl.IsCharServerRunning();
            bool chatServerRunning = _dboServerControl.IsChatServerRunning();
            bool gameServerRunning = _dboServerControl.IsGameServerRunning();
            bool authServerRunning = _dboServerControl.IsAuthServerRunning();

            model.ServerDirectoryPath = _appConfig.Dbo.RootPath;
            model.MasterServerPath = Path.Combine(_appConfig.Dbo.RootPath, "MasterServer.exe");
            model.QueryServerPath = Path.Combine(_appConfig.Dbo.RootPath, "GameServer.exe");
            model.CharServerPath = Path.Combine(_appConfig.Dbo.RootPath, "CharServer.exe");
            model.ChatServerPath = Path.Combine(_appConfig.Dbo.RootPath, "ChatServer.exe");
            model.GameServerPath = Path.Combine(_appConfig.Dbo.RootPath, "GameServer.exe");
            model.AuthServerPath = Path.Combine(_appConfig.Dbo.RootPath, "AuthServer.exe");

            model.MasterServerPid = masterServerRunning ? _dboServerControl.GetMasterServerPid() : 0;
            model.QueryServerPid = queryServerRunning ? _dboServerControl.GetQueryServerPid() : 0;
            model.CharServerPid = charServerRunning ? _dboServerControl.GetCharServerPid() : 0;
            model.ChatServerPid = chatServerRunning ? _dboServerControl.GetChatServerPid() : 0;
            model.GameServerPid = gameServerRunning ? _dboServerControl.GetGameServerPid() : 0;
            model.AuthServerPid = authServerRunning ? _dboServerControl.GetAuthServerPid() : 0;

            model.MasterServerStatus = masterServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
            model.QueryServerStatus = queryServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
            model.CharServerStatus = charServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
            model.ChatServerStatus = chatServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
            model.GameServerStatus = gameServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
            model.AuthServerStatus = authServerRunning ? DboServerAppStatus.Online : DboServerAppStatus.Offline;
        }

        public JsonResult DboServerControlActions()
        {
            bool masterServerRunning = _dboServerControl.IsMasterServerRunning();
            bool queryServerRunning = _dboServerControl.IsQueryServerRunning();
            bool charServerRunning = _dboServerControl.IsCharServerRunning();
            bool chatServerRunning = _dboServerControl.IsChatServerRunning();
            bool gameServerRunning = _dboServerControl.IsGameServerRunning();
            bool authServerRunning = _dboServerControl.IsAuthServerRunning();

            return Json(new { 
                masterServerRunning,
                queryServerRunning,
                charServerRunning, 
                chatServerRunning,
                gameServerRunning,
                authServerRunning
            });
        }

        [HttpPost]
        public JsonResult DboServerControlStopServer([FromQuery] string id)
        {
            string error;
            bool success;
            switch (id)
            {
                case DboServerControlViewModel.AuthServerId: success = _dboServerControl.StopAuthServer(out error); break;
                case DboServerControlViewModel.GameServerId: success = _dboServerControl.StopGameServer(out error); break;
                case DboServerControlViewModel.MasterServerId: success = _dboServerControl.StopMasterServer(out error); break;
                case DboServerControlViewModel.QueryServerId: success = _dboServerControl.StopQueryServer(out error); break;
                case DboServerControlViewModel.CharServerId: success = _dboServerControl.StopCharServer(out error); break;
                case DboServerControlViewModel.ChatServerId: success = _dboServerControl.StopChatServer(out error); break;
                default: success = false; error = "Unknown server id."; break;
            }

            return Json(new { status = success, error = error });
        }

        [HttpPost]
        public JsonResult DboServerControlStartServer([FromQuery] string id)
        {
            string error;
            bool success;
            switch (id)
            {
                case DboServerControlViewModel.AuthServerId: success = _dboServerControl.StartAuthServer(out error); break;
                case DboServerControlViewModel.GameServerId: success = _dboServerControl.StartGameServer(out error); break;
                case DboServerControlViewModel.MasterServerId: success = _dboServerControl.StartMasterServer(out error); break;
                case DboServerControlViewModel.QueryServerId: success = _dboServerControl.StartQueryServer(out error); break;
                case DboServerControlViewModel.CharServerId: success = _dboServerControl.StartCharServer(out error); break;
                case DboServerControlViewModel.ChatServerId: success = _dboServerControl.StartChatServer(out error); break;
                default: success = false; error = "Unknown server id."; break;
            }

            return Json(new { status = success, error = error });
        }

        [HttpGet]
        public ActionResult BanList()
        {
            var model = new BannedUsersViewModel
            {
                BanList = new List<BannedUserListItem>()
            };
            var bans = _authDb.AccountBans
                .Include(u => u.GameAccount)
                .ToList();


            var bnetAccounts = bans.Select(x => x.GameAccount.BnetAccountId).ToList();
            var users = _freedomDb.Users.Where(x => bnetAccounts.Contains(x.BnetAccountId));
            foreach (var bannedUser in bans)
            {
                var freedomUser = users.FirstOrDefault(x => x.BnetAccountId == bannedUser.GameAccount.BnetAccountId);
                model.BanList.Add(new BannedUserListItem()
                {
                    UserId = freedomUser?.BnetAccountId ?? -1,
                    Username = freedomUser?.UserName,
                    DisplayName = freedomUser?.DisplayName,
                    Active = bannedUser.Active,
                    BanDate = bannedUser.BanDate.ToString(),
                    UnbanDate = bannedUser.Unbandate.ToString(),
                    BannedBy = bannedUser.BannedBy,
                    BanReason = bannedUser.BanReason
                });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> BanAccounts(int? id)
        {
            var user = await _userManager.FindByIdAsync(id?.ToString());
            _dataLoader.LoadExtraUserData(user);
            if (user == null)
            {
                SetAlertMsg($"Account with id: {id?.ToString()} could not be found.", AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var model = new BanGameAccountsViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                BnetAccountId = user.UserData.BnetAccount.Id,
                Duration = BanDuration.OneDay
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult BanAccounts(BanGameAccountsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _accountManager.BanGameAccounts(model.BnetAccountId, model.Duration, model.Reason, User.Identity.Name);

            SetAlertMsg("Game accounts have been sucessfully banned!", AlertMsgType.AlertSuccess);
            return RedirectToAction("BanList", "Admin");
        }

        [HttpPost]
        public async Task<ActionResult> UnBanAccounts(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                SetAlertMsg($"Account with id: {userId} could not be found.", AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            _accountManager.UnbanGameAccounts(user.BnetAccountId);
            SetAlertMsg("Game accounts have been sucessfully unbanned!", AlertMsgType.AlertSuccess);
            return RedirectToAction("BanList", "Admin");
        }

        [HttpGet]
        public async Task<ActionResult> PasswordReset(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Scheme);
            return View(new PasswordResetTokenViewModel()
            {
                ResetLink = resetLink,
                Username = user.UserName
            });
        }
    }
}
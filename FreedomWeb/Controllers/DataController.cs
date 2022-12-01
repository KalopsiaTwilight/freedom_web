﻿using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomUtils.DataTables;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Admin;
using FreedomWeb.ViewModels.Home;
using FreedomWeb.ViewModels.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{ 
    [Authorize]
    public class DataController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly CharacterManager _characterManager;
        private readonly ServerControl _serverControl;
        private readonly CommandStore _commandStore;
        private readonly ExtraDataLoader _dataLoader;

        public DataController(UserManager<User> userManager, CharacterManager characterManager, ServerControl serverControl, CommandStore commandStore, ExtraDataLoader dataLoader)
        {
            _userManager = userManager;
            _characterManager = characterManager;
            _serverControl = serverControl;
            _commandStore = commandStore;
            _dataLoader = dataLoader;
        }

        /// <summary>
        /// Command List data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<JsonResult> CommandListData(DTParameterModel parameters)
        {
            //var user = await _userManager.FindByIdAsync(CurrentUserId);

            int total = 0;
            int filtered = 0;
            var currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(currentUser);
            var gmLevel = currentUser.UserData.GameAccountAccess.GMLevel;
            if (User.IsInRole(FreedomRole.RoleAdmin))
            {
                gmLevel = GMLevel.Unused;
            }

            var list = _commandStore.DTGetFilteredAvailableFreedomCommands(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    gmLevel
                );

            return Json(new DTResponseModel() {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = filtered,
                data = list
            });
        }

        /// <summary>
        /// Online character list data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> OnlineListData([FromForm] DTParameterModel parameters)
        {
            var user = await _userManager.FindByIdAsync(CurrentUserId);       
            bool allowUsernameViewing = (user == null ? false : user.FreedomRoles.Where(r => r.Name == FreedomRole.RoleAdmin).Any());

            int total = 0;
            int filtered = 0;
            var charList = _characterManager.DTGetFilteredOnlineCharacters(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    allowUsernameViewing
                );

            var statusCharList = new List<StatusCharListItem>();

            foreach (var character in charList)
            {
                var raceIconPath = character.Gender == CharGender.Male ? character.CharData.RaceData.IconMalePath : character.CharData.RaceData.IconFemalePath;
                statusCharList.Add(new StatusCharListItem()
                {
                    UserId = character.CharData.WebUser.Id,
                    FactionIconPath = character.CharData.RaceData.IconFactionPath,
                    Name = character.Name,
                    Owner = character.CharData.WebUser.DisplayName,
                    OwnerUsername = allowUsernameViewing ? character.CharData.WebUser.UserName : "",
                    Race = character.CharData.RaceData.Name,
                    RaceIconPath = raceIconPath,
                    Class = character.CharData.ClassData.Name,
                    ClassIconPath = character.CharData.ClassData.IconPath,
                    Gender = Enum.GetName(character.Gender.GetType(), character.Gender),
                    MapName = !_characterManager.IsGMOn(character.Id) ? character.CharData.MapName : (allowUsernameViewing ? "(" + character.CharData.MapName + ")" : "(Hidden)"), //Kret
                    ZoneName = !_characterManager.IsGMOn(character.Id) ? character.CharData.ZoneName : (allowUsernameViewing ? "(" + character.CharData.ZoneName + ")" : "(Hidden)"), //Kret
                    Latency = character.Latency
                });
            }

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = filtered,
                data = statusCharList
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StatusLinePartial()
        {
            var model = new StatusViewModel();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool worldServerOnline = _serverControl.IsWorldServerOnline();

            if (!bnetServerRunning && !worldServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.Offline;
            }
            else if (worldServerRunning && !worldServerOnline)
            {
                model.Status = EnumFreedomGameserverStatus.WorldLoading;
            }
            else if (!worldServerRunning && bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.WorldDown;
            }
            else if (worldServerRunning && !bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.LoginDown;
            }
            else
            {
                model.Status = EnumFreedomGameserverStatus.Online;
            }

            return PartialView("_StatusLinePartial", model);
        } 
    }
}
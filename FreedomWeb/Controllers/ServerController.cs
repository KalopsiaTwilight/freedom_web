﻿using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using FreedomUtils.MvcUtils;
using FreedomLogic.Managers;
using System.Threading.Tasks;
using FreedomWeb.Models;
using FreedomLogic.Entities;
using FreedomLogic.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FreedomWeb.Controllers
{
    [Authorize]
    public class ServerController : FreedomController
    {
        private readonly ServerManager _serverManager;
        public ServerController(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpGet]
        public ActionResult CommandList()
        {
            return View();
        }
    }
}
using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Resources;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using FreedomUtils.LINQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.Managers
{
    public class ServerManager
    {
        private readonly DbWorld _worldDb;
        private readonly DbFreedom _freedomDb;

        public ServerManager(DbWorld worldDb, DbFreedom freedomDb)
        {
            _worldDb = worldDb;
            _freedomDb = freedomDb;
        }
    }
}

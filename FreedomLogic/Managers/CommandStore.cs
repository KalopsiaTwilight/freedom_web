using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FreedomLogic.Managers
{
    public class CommandStore
    {
        private readonly DbFreedom _freedomDb;
        public CommandStore(DbFreedom freedomDb)
        {
            _freedomDb = freedomDb;
        }

        public List<FreedomCommand> GetAvailableFreedomCommands(GMLevel gmlevel)
        {
            return _freedomDb.Commands
                 .Where(c => ((int)c.GMLevel) <= ((int)gmlevel))
                 .OrderBy(c => c.Command)
                 .ToList();
        }

        public List<FreedomCommand> DTGetFilteredAvailableFreedomCommands(
            ref int recordsTotal,
            ref int recordsFiltered,
            int start,
            int length,
            IEnumerable<DTColumn> columnList,
            IEnumerable<DTOrder> orderList,
            GMLevel gmlevel)
        {
            var list = new List<FreedomCommand>();

            // [0] - Command
            // [1] - Syntax
            // [2] - Description
            // [3] - GMLevel
            var colArray = columnList.ToArray();
            var orderArray = orderList.ToArray();

            // Can't use array in LINQ directly, because: http://stackoverflow.com/a/8354049
            var filterCommand = colArray[0].Search.Value;
            var filterSyntax = colArray[1].Search.Value;
            var filterDescription = colArray[2].Search.Value;
            var filterGmLevelDisplay = colArray[3].Search.Value;

            // Set up basic filter query parts
            var query = _freedomDb.Commands
                .Where(c => ((int)c.GMLevel) <= ((int)gmlevel));
            if (!string.IsNullOrEmpty(filterCommand))
            {
                query = query.Where(c => c.Command.ToUpper().Contains(filterCommand.ToUpper()));
            }
            if (!string.IsNullOrEmpty(filterSyntax))
            {
                query = query.Where(c => c.Syntax.ToUpper().Contains(filterSyntax.ToUpper()));
            }
            if (!string.IsNullOrEmpty(filterGmLevelDisplay))
            {
                query = query.Where(c => c.Description.ToUpper().Contains(filterDescription.ToUpper()));
            }

            // Can't filter GMLevel through display value directly, so we enumerate through all GMLevel enums 
            // and filter out non-matching ones from query
            foreach (GMLevel value in Enum.GetValues(typeof(GMLevel)))
            {
                if (!string.IsNullOrEmpty(filterGmLevelDisplay) && value.DisplayName().Contains(filterGmLevelDisplay.ToUpper(), StringComparison.OrdinalIgnoreCase))
                    query = query.Where(c => c.GMLevel != value);
            }

            // Column ordering
            bool nextLevel = false;
            IOrderedQueryable<FreedomCommand> orderedQuery = null;

            foreach (var order in orderArray)
            {
                string colName = colArray[order.Column].Data;
                if (!nextLevel)
                {
                    if (order.Dir == "desc")
                    {
                        orderedQuery = query.OrderByDescending(c => EF.Property<object>(c, colName));
                    }
                    else
                    {
                        orderedQuery = query.OrderBy(c => EF.Property<object>(c, colName));
                    }
                    nextLevel = true;
                }
                else
                {
                    if (order.Dir == "desc")
                    {
                        orderedQuery = orderedQuery.ThenByDescending(c => EF.Property<object>(c, colName));
                    }
                    else
                    {
                        orderedQuery = orderedQuery.ThenBy(c => EF.Property<object>(c, colName));
                    }
                }
            }

            // Load and set results
            list = orderedQuery.Skip(start).Take(length).ToList();
            recordsTotal = _freedomDb.Commands.Where(c => ((int)c.GMLevel) <= ((int)gmlevel)).Count();
            recordsFiltered = orderedQuery.Count();

            return list;
        }

    }
}

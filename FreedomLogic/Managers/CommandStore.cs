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
            GMLevel gmlevel,
            string search = "")
        {
            var list = new List<FreedomCommand>();

            var orderArray = orderList.ToArray();
            var colArray = columnList.ToArray();

            var matchingGmLevels = Enum.GetValues<GMLevel>()
                .Where(l => l.DisplayName().ToUpper().Contains(search))
                .ToArray();

            // Set up basic filter query parts
            var query = _freedomDb.Commands
                .Where(c => ((int)c.GMLevel) <= ((int)gmlevel))
                .Where(c => c.Command.ToUpper().Contains(search.ToUpper()) 
                         || c.Syntax.ToUpper().Contains(search.ToUpper())
                         || c.Description.ToUpper().Contains(search.ToUpper())
                         || matchingGmLevels.Contains(c.GMLevel)
                );

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

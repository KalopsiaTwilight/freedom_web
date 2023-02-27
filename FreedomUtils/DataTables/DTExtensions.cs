using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomUtils.DataTables
{
    public static class DTExtensions
    {
        public static IQueryable<T> ApplyDataTableParameters<T>(this IQueryable<T> queryable, DTParameterModel parameters, Dictionary<string, string> advancedMappings = null)
        {
            return queryable.OrderByDataTable(parameters, advancedMappings).FilterByDataTable(parameters);
        }


        public static IQueryable<T> OrderByDataTable<T>(this IQueryable<T> queryable, DTParameterModel parameters, Dictionary<string, string> advancedMappings = null)
        {
            IOrderedQueryable<T> orderedQuery = null;
            advancedMappings ??= new();

            bool nextLevel = false;
            foreach (var order in parameters.Order)
            {
                string colName = parameters.Columns.ElementAt(order.Column).Data;
                if (advancedMappings.ContainsKey(colName))
                {
                    colName = advancedMappings[colName];
                }
                if (!nextLevel)
                {
                    orderedQuery = (order.Dir == "desc") 
                        ? queryable.OrderByDescending(c => EF.Property<T>(c, colName)) 
                        : queryable.OrderBy(c => EF.Property<T>(c, colName));
                    nextLevel = true;
                }
                else
                {
                    orderedQuery = (order.Dir == "desc")
                        ? orderedQuery.ThenByDescending(c => EF.Property<T>(c, colName))
                        : orderedQuery.ThenBy(c => EF.Property<T>(c, colName));
                }
            }
            return orderedQuery ?? queryable;
        }

        public static IQueryable<T> FilterByDataTable<T>(this IQueryable<T> queryable, DTParameterModel parameters)
        {
            return queryable.Skip(parameters.Start).Take(parameters.Length);
        }
    }
}

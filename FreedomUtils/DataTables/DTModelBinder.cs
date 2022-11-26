using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// From http://stackoverflow.com/a/28223358
/// </summary>
namespace FreedomUtils.DataTables
{
    /// <summary>
    /// Model Binder for DTParameterModel (DataTables)
    /// </summary>
    public class DTModelBinder : IModelBinder
    {       
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;
            // Retrieve request data
            var draw = Convert.ToInt32(request.Query["draw"]);
            var start = Convert.ToInt32(request.Query["start"]);
            var length = Convert.ToInt32(request.Query["length"]);
            // Search
            var search = new DTSearch
            {
                Value = request.Query["search[value]"],
                Regex = Convert.ToBoolean(request.Query["search[regex]"])
            };
            // Order
            var o = 0;
            var order = new List<DTOrder>();
            while (!string.IsNullOrEmpty(request.Query["order[" + o + "][column]"]))
            {
                order.Add(new DTOrder
                {
                    Column = Convert.ToInt32(request.Query["order[" + o + "][column]"]),
                    Dir = request.Query["order[" + o + "][dir]"]
                });
                o++;
            }
            // Columns
            var c = 0;
            var columns = new List<DTColumn>();
            while (!string.IsNullOrEmpty(request.Query["columns[" + c + "][name]"]))
            {
                columns.Add(new DTColumn
                {
                    Data = request.Query["columns[" + c + "][data]"],
                    Name = request.Query["columns[" + c + "][name]"],
                    Orderable = Convert.ToBoolean(request.Query["columns[" + c + "][orderable]"]),
                    Searchable = Convert.ToBoolean(request.Query["columns[" + c + "][searchable]"]),
                    Search = new DTSearch
                    {
                        Value = request.Query["columns[" + c + "][search][value]"],
                        Regex = Convert.ToBoolean(request.Query["columns[" + c + "][search][regex]"])
                    }
                });
                c++;
            }

            bindingContext.Result = ModelBindingResult.Success(new DTParameterModel
            {
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns
            });
            return Task.CompletedTask;
        }
    }
}

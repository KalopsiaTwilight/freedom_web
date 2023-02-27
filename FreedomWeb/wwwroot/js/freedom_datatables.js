(function ($) {
    // Simple wrapped DataTable helper (for small-scale, non-serverside data)
    $.fn.FreedomSimpleDataTable = function (options) {        
        var defaultOptions = {
            "lengthMenu": [[25, 50, 100], [25, 50, 100]],
            "initComplete": function () {
                var table = this;

                if (table.parent().parent().hasClass('filtered-table'))
                {
                    var filterListElem = table.parent().parent().find(".filter-list");

                    this.api().columns().every(function () {
                        var column = this;
                        var header = $(column.header());
                        if (header.hasClass('nosearch'))
                            return;

                        var input = $('<input/>', {
                            'class': 'form-control',
                            'type': 'search',
                            'placeholder': "Search: " + column.title()
                        });
                        input.appendTo(filterListElem);
                        input.on('keyup change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^.*' + val + '.*$' : '', true, false)
                                .draw();
                        });
                    });
                }                
            },
            "dom": '<"top">rt<"row" <"col-md-4"l><"col-md-4"i><"col-md-4"p>><"clear">',
            "columnDefs": [
                {
                    "sortable": false,
                    "targets": 'nosort'
                },
                {
                    "searchable": false,
                    "targets": 'nosearch'
                },
            ],
        };

        var mergedOptions = $.extend(true, defaultOptions, options);
        return this.DataTable(mergedOptions);
    };

    // Serverside wrapped DataTable helper
    // Must specify "dataUrl" key with local URL to the data
    $.fn.FreedomServerDataTable = function (options) {
        var defaultOptions = {
            "lengthMenu": [[25, 50, 100], [25, 50, 100]],
            "serverSide": true,
            "ajax": {
                "type": "POST",
                "url": options.dataUrl,
            }
        };

        var mergedOptions = $.extend(true, defaultOptions, options);
        this.on("preInit", function () {
            console.log("preInit");
        });
        var dt = this.DataTable(mergedOptions);
        dt.on('processing.dt', function (e, settings, processing) {
            if (processing) {
                var processingParent = $("<div class='dataTables_processing' style=''>");
                var processingCard = $("<div class='bg-text-light card' style='height:100px;width:250px;'>");
                processingCard.append("<i class='fa fa-spinner fa-spin fa-5x' style='margin-top:8px;'></i>");
                processingParent.append(processingCard);
                $(e.target).parent().append(processingParent);
            } else {
                $(this).parent().find(".dataTables_processing").remove();
            }
        })
        return dt;
    };

}(jQuery));
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
            "oLanguage": {
                "sProcessing": "<i class='fa fa-spinner fa-spin fa-5x'></i>"
            },
            "lengthMenu": [[25, 50, 100], [25, 50, 100]],
            "processing": true,
            "serverSide": true,
            "initComplete": function () {
                var table = this;

                if (table.parent().parent().hasClass('filtered-table')) {
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
                                .search(val ? val : '', false, false)
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
                {
                    "orderable": false,
                    "targets": 'noorder'
                },
            ],
            "ajax": {
                "type": "POST",
                "url": options.dataUrl,
            }
        };

        var mergedOptions = $.extend(true, defaultOptions, options);
        return this.DataTable(mergedOptions);
    };

}(jQuery));
// from https://www.datatables.net/plug-ins/api/column().title()
$.fn.dataTable.Api.register('column().title()', function () {
    var colheader = this.header();
    return $(colheader).text().trim();
});

function InitUI()
{

}
$(function () {
    /*
     * ALERT FADEOUT SCRIPT
     */
    $(".fade-out-toggle").fadeTo(5000, 500).slideUp(500, function () {
        $(".fade-out-toggle").alert('close');
    });

    /*
     * SIDEBAR SCRIPTS
     */

    /* == menu group collapse scripts == */

    $('#menuSubGeneral').collapse({ 'toggle': false });
    $('#menuSubServerDatabase').collapse({ 'toggle': false });
    $('#menuSubAccount').collapse({ 'toggle': false });
    $('#menuSubAdmin').collapse({ 'toggle': false });

    if (typeof (Storage) !== "undefined") {
        $('.sidebar .nav-header > .panel-body').each(function () {
            var collapseElem = $(this);
            var id = collapseElem.attr('id');
            collapsed = window.localStorage.getItem('sidebar.collapsed#' + id) || "false";

            if (collapsed === "true") {
                collapseElem.collapse('hide');
                collapseElem.parent().find(".chevron-toggle").removeClass("glyphicon-chevron-down");
                collapseElem.parent().find(".chevron-toggle").addClass("glyphicon-chevron-right");
            } else {
                collapseElem.collapse('show');
                collapseElem.parent().find(".chevron-toggle").removeClass("glyphicon-chevron-right");
                collapseElem.parent().find(".chevron-toggle").addClass("glyphicon-chevron-down");
            }
        });
    }

    function saveSidebarCollapseState(id, collapsed) {
        if (typeof (Storage) !== "undefined") {
            window.localStorage.setItem('sidebar.collapsed#' + id, collapsed);
        }
    }

    $('.sidebar .nav-header > .panel-body').on('hide.bs.collapse', function () {
        var collapseElem = $(this);
        saveSidebarCollapseState(collapseElem.attr('id'), true);
        collapseElem.parent().find(".chevron-toggle").removeClass("glyphicon-chevron-down");
        collapseElem.parent().find(".chevron-toggle").addClass("glyphicon-chevron-right");
    });

    $('.sidebar .nav-header > .panel-body').on('show.bs.collapse', function () {
        var collapseElem = $(this);
        saveSidebarCollapseState(collapseElem.attr('id'), false);
        collapseElem.parent().find(".chevron-toggle").removeClass("glyphicon-chevron-right");
        collapseElem.parent().find(".chevron-toggle").addClass("glyphicon-chevron-down");
    });
    
});
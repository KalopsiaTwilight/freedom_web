$(function () {
    /*
     * ALERT FADEOUT SCRIPT
     */
    $(".fade-out-toggle").fadeTo(5000, 500).slideUp(500, function () {
        $(".fade-out-toggle").alert('close');
    });
});
$(document).on('click', '.yamm .dropdown-menu', function (e) {
    e.stopPropagation()
})
$(window).resize(function () {
    if ($(window).width() <= 768) {
        $('.navbarclass').attr("id", "navbartext");
    } else {
        $('.navbarclass').attr("id", "");
    }
});
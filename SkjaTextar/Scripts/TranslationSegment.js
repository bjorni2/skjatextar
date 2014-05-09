$(document).ready(function () {
    $(".edit").click(function () {
        // Create a JSON object:
        var button = $(this).children().first();
        var input = button.parent().parent().parent().children().first();
        if(button.hasClass("glyphicon-edit")) {
            button.removeClass("glyphicon-edit").addClass("glyphicon-floppy-disk");
            input.removeAttr("readonly");
        }
        else {
            button.removeClass("glyphicon-floppy-disk").addClass("glyphicon-edit");
            input.attr("readonly", "readonly");
        }
        
    });
});
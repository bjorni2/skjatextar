$(document).ready(function () {
    $(".edit").click(function () {
        // Create a JSON object:
        var button = $(this).children().first();
        var input = button.parent().parent().parent().children().first();
        if(button.hasClass("glyphicon-edit")) {
            button.removeClass("glyphicon-edit").addClass("glyphicon-floppy-disk");
            input.removeAttr("readonly");
            input.focus();
        }
        else {
            button.removeClass("glyphicon-floppy-disk").addClass("glyphicon-edit");
            input.attr("readonly", "readonly");
            var segmentID = $(this).attr("data-segmentid");
            var translationID = $("#translationid").attr("data-translationid");
            var line = $(this).attr("data-line");
            var translationText = input.val();
            var args = { "translationID": translationID, "segmentID": segmentID, "translationText": translationText, "line": line };
            $.post("/Translation/UpdateLine", args, null);
        }
        
    });
});

$(document).keypress(function (e) {
    if (e.which == 13) {
         $(':focus').siblings().first().children().first().trigger( "click" );
    }
});
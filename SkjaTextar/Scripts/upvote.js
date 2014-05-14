/*$(document).ready(function () {
    $(".glyphicon-chevron-up").click(function () {
        // Create a JSON object:
        var button = $(this).parent().first();
        if (button.hasClass("btn-success")) {
            return false;
           // button.removeClass("glyphicon-edit").addClass("glyphicon-floppy-disk");
            //input.removeAttr("readonly");
            //input.focus();
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
        $(':focus').siblings().first().children().first().trigger("click");
        var readOnly = $(':focus').attr('readonly');
        if (!readOnly) {
            $(':focus').select();
            return false;
        }
        $(':focus').parent().next().next().next().children().first().siblings('span').children().first().trigger("click");
    }
});*/
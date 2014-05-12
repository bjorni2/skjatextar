$(document).ready(function () {
    $('#upload').bind("click", function () {
        var srtFile = $('#file').val();
        if (srtFile == '') {
            if (!confirm("Engin .srt skrá valin\nErtu viss um að þú viljir stofna tóma þýðingu?")) {
                return false;
            }
        }
    });
});
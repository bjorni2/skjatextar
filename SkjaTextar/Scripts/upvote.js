$(document).ready(function () {
    $(".upvote").click(function () {
        var usrLgdIn = $("#logged-in").attr("data-user-logged-in");
        alert(usrLgdIn);
        if (usrLgdIn == "false"){
            return false;
        }
        if ($(this).hasClass("btn-success")) {
            return false;
        }
        else {
            var scoreToAdd = 1;
            var downVote = $(this).next().next().next();
            if (downVote.hasClass("btn-danger")) {
                downVote.removeClass("btn-danger");
                scoreToAdd = 2;
            }
            $(this).addClass("btn-success");

            var vote = $(this).prev().attr("data-vote");
            var id = $(this).prev().attr("data-id");
            var args = { "id": id, "vote": vote };
            var score = parseInt($(this).next().html());
            score += scoreToAdd;
            $(this).next().html(score);
            $.post("/Request/VoteAjax", args, null);
        }
    });
    $(".downvote").click(function () {
        var usrLgdIn = $("#logged-in").attr("data-user-logged-in");
        if (usrLgdIn == "false") {
            return false;
        }
        if ($(this).hasClass("btn-danger")) {
            return false;
        }
        else {
            var scoreToAdd = -1;
            var upVote = $(this).prev().prev().prev();
            if (upVote.hasClass("btn-success")) {
                upVote.removeClass("btn-success");
                scoreToAdd = -2;
            }
            $(this).addClass("btn-danger");

            var vote = $(this).prev().attr("data-vote");
            var id = $(this).prev().attr("data-id");
            var args = { "id": id, "vote": vote };
            var score = parseInt($(this).prev().prev().html());
            score += scoreToAdd;
            $(this).prev().prev().html(score);
            $.post("/Request/VoteAjax", args, null);
        }
    });
});
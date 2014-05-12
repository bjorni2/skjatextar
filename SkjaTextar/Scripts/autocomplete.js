
$(function () {
    $("#main-search-bar").autocomplete({
        minLength: 2,
        source: "/Home/AutoComplete",
        select: function (event, ui) {
            document.location = "/Media/Index/" + ui.item.id ;
        },
        _renderItem: function (ul, item) {
            return $("<li>")
            .attr("data-value", item.value)
            .append($("<a>").text(item.label))
            .appendTo(ul);
        }
    })
});
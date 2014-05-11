$(document).ready(function () {
    var mediatype = $("#mediatype").attr("data-mediatype");
    var url = 'https://api.themoviedb.org/3/',
        mode = 'search/' + mediatype + '?query=',
        input,
        movieName,
        key = '&api_key=8fffe85c028b50603443d9a67c9fa8ee';

    // /3/movie/{id}/images

    var input = $("#mediatitle").attr("data-title");
    var year = $("#relyear").attr("data-year");
    movieName = encodeURI(input);
    $.ajax({
        type: 'GET',
        url: url + mode + input + key + '&year=' + year,
        async: false,
        jsonpCallback: 'testing',
        contentType: 'application/json',
        dataType: 'jsonp',
        success: function (json) {
            console.dir(json);
            if (json.results[0] != null) {
                var id = json.results[0].id;
                $.ajax({
                    type: 'GET',
                    url: 'https://api.themoviedb.org/3/' + mediatype + '/' + id + '/images?api_key=8fffe85c028b50603443d9a67c9fa8ee',
                    async: false,
                    jsonpCallback: 'testing',
                    contentType: 'application/json',
                    dataType: 'jsonp',
                    success: function (json2) {
                        console.dir(json2);
                        $("#poster").html('<img id="thePoster" src=https://image.tmdb.org/t/p/w185' + json2.posters[0].file_path + ' />');
                    },
                    error: function (e) {
                        console.log(e.message);
                    }
                });
            }
            else {
                $.ajax({
                    type: 'GET',
                    url: url + mode + input + key,
                    async: false,
                    jsonpCallback: 'testing',
                    contentType: 'application/json',
                    dataType: 'jsonp',
                    success: function (json) {
                        console.dir(json);
                        if (json.results[0] != null) {
                            var id = json.results[0].id;
                            $.ajax({
                                type: 'GET',
                                url: 'https://api.themoviedb.org/3/' + mediatype + '/' + id + '/images?api_key=8fffe85c028b50603443d9a67c9fa8ee',
                                async: false,
                                jsonpCallback: 'testing',
                                contentType: 'application/json',
                                dataType: 'jsonp',
                                success: function (json2) {
                                    console.dir(json2);
                                    $("#poster").html('<img id="thePoster" src=https://image.tmdb.org/t/p/w185' + json2.posters[0].file_path + ' />');
                                },
                                error: function (e) {
                                    console.log(e.message);
                                }
                            });
                        }
                    },
                    error: function (e) {
                        console.log(e.message);
                    }

                });
            }
        },
        error: function (e) {
            console.log(e.message);
        }

    });
});
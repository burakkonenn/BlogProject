
    $(function () {
        var flag = false;

    $(".container").on("click", "#LikeButton", function () {
            var btn = $(this)
    var id = btn.data("id")
    flag = !flag;
    event.stopPropagation();

    $.ajax({
        type: "GET",
    url: "/Users/Users/like/" + id,
    success: function (data) {
                    var like = parseInt($("#span").text());
    if (flag) {
        like += 1;
                    }
    else {
        like -= 1;
                    }
    $("#span").text(like);
    console.log(like);
    //$("#span").html(data);
    btn.attr('id', 'UnLike')
                }
            })
        })
    })

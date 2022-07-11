
    $(function () {
        $(".container").on("click", "#UnLike", function () {
            var btn = $(this)
            var id = btn.data("id")

            $.ajax({
                type: "GET",
                url: "/Users/Users/unlike/" + id,
                success: function (data) {
                    btn.attr('id', 'LikeButton')
                }
            })
        })
    })

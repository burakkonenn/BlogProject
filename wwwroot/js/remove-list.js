
    $(function () {
        //var flag = false;

        $(".container").on("click", "#remove-list", function () {
            var btn = $(this)
            var id = btn.data("id")


            $.ajax({
                type: "GET",
                url: "/Users/Users/removelist/" + id,
                success: function (data) {
                    //var like = parseInt($("#span").text());
                    //if (flag) {
                    //    like += 1;
                    //}
                    //else {
                    //    like -= 1;
                    //}
                    //$("#span").text(like);
                    console.log(data);
                    //$("#span").html(data);
                    btn.attr('id', 'save-button')
                }
            })
        })
    })

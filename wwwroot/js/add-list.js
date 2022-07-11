
    $(function () {
        //var flag = false;

        $(".container").on("click", "#save-button", function () {
            var btn = $(this)
            var id = btn.data("id")


            $.ajax({
                type: "GET",
                url: "/Users/Users/savelist/" + id,
                success: function (data) {

                    console.log(data);
                    var bookmark = "";
                    bookmark += `<i class="fa-solid fa-bookmark"></i>`;

                    $(".bookmark").html(bookmark);
                    //$("#span").html(data);
                    btn.attr('id', 'remove-list')
                }
            })
        })
    })

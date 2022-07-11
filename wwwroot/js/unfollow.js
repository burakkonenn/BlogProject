
    $(function() {
        $(".container").on("click", "#followed", function() {
        var btn = $(this)
            var id = btn.data("id")

            $.ajax({
            type: "GET",
                headers:
            {
                "RequestVerificationToken": '@GetAntiXsrfRequestToken()'
                },
                url: "/Users/Users/IsFollowing/" + id,
                success: function(data) {
                btn.text('Takip et')
                    btn.attr('id', 'follow')
                }
        })
        })
    })



    $(".container").on("click", "#follow", function () {
        var btn = $(this)
    var id = btn.data("id")

    var t = $("input[name='__RequestVerificationToken']").val()

    $.ajax({
        type: "GET",
    headers:
    {
        "RequestVerificationToken": '@GetAntiXsrfRequestToken()'
                },
    url: "/Users/Users/UserId/" + id,
    success: function (data) {
        btn.text('Takip ediliyor')
                    btn.attr('id', 'followed')
    btn.css({width: '100px' });
                },
            });
    });


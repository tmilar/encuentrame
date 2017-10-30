(function () {
    $(document).ready(function () {

        var $url = $('#url');
        var $username = $('#username');
        var $password= $('#password');
        var $token = $('#token');
        var $user = $('#user');

        var $buttonLogin = $('#login');

        var $image = $('#image');
        var $buttonUploadImage = $('#uploadImage');

        var $method = $('#method');
        var $action = $('#action');
        var $text = $('#parameters');
        var $result = $('#result');
        var $buttonRun = $('#run');

        $buttonUploadImage
            .on('click',
                function () {
                    var data = new FormData();
                    var files = $image.get(0).files;
                    if (files.length > 0) {
                        data.append("image", files[0]);
                    }
                    $.ajax({
                        url: $url.val() + '/account/UploadImage',
                        headers: {
                            'token': $token.val(),
                            'user': $user.val()
                        },
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        data: data,
                        success: function (response) {
                            $result.val('Upload OK');
                        },
                        error: function (er) {
                            console.log(er);
                        }
                    });

                 
                });

        $buttonLogin.on('click',
            function () {

                $.ajax({
                        url: $url.val() + '/authentication/login',
                        type: 'POST',
                        data: {
                            "Username": $username.val(),
                            "Password": $password.val()
                        },
                        dataType: 'json'

                    }).done(function (data) {
                        $token.val(data.Token);
                        $user.val(data.UserId);
                    })
                    .fail(function (x, y, z) {

                        $result.val(x.responseText);
                    });
            });

        $buttonRun.on('click',
            function () {

                $.ajax({
                        url: $url.val() + $action.val(),
                        headers: {
                            'token': $token.val(),
                            'user': $user.val()
                        },
                        type: $method.val(),
                        data: jQuery.parseJSON($text.val()),
                        dataType:'json'

                    }).done(function (data) {
                        
                        $result.val(JSON.stringify(data, null, 4));
                    })
                    .fail(function (x, y, z) {
                        
                        $result.val(x.responseText);
                    });
            });

       

    });
})();
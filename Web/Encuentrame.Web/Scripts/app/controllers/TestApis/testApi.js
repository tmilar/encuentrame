(function () {
    $(document).ready(function () {

        var $url = $('#url');
        var $username = $('#username');
        var $password= $('#password');
        var $token = $('#token');
        var $user = $('#user');

        var $buttonLogin = $('#login');

        var $methodPost = $('#methodPost');
        var $textPost = $('#parametersPost');
        var $resultPost = $('#resultPost');
        var $buttonPost = $('#post');

        var $methodGet = $('#methodGet');
        var $textGet = $('#parametersGet');
        var $resultGet = $('#resultGet');
        var $buttonGet = $('#get');


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

                        $resultPost.val(x.responseText);
                    });
            });

        $buttonPost.on('click',
            function () {

                $.ajax({
                        url: $url.val() + $methodPost.val(),
                        headers: {
                            'token': $token.val(),
                            'user': $user.val()
                        },
                        type: 'DELETE',
                        data: jQuery.parseJSON($textPost.val()),
                        dataType:'json'

                    }).done(function (data) {
                        
                        $resultPost.val(JSON.stringify(data));
                    })
                    .fail(function (x, y, z) {
                        
                        $resultPost.val(x.responseText);
                    });
            });

        $buttonGet.on('click',
            function () {
                $.ajax({
                        url: $url.val() + $methodGet.val(),
                        headers: {
                            'token': $token.val(),
                            'user': $user.val()
                        },
                        data: jQuery.parseJSON($textGet.val()),

                    })
                    .done(function (data) {
                        $resultGet.val(JSON.stringify(data));
                    })
                    .fail(function (x, y, z) {
                        $resultGet.val(x.responseText);
                    });
            });

    });
})();
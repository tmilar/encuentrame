(function () {
    $(document).ready(function () {

        var $url = $('#url');
        var $token = $('#token');
        var $user = $('#user');

        var $methodPost = $('#methodPost');
        var $textPost = $('#parametersPost');
        var $resultPost = $('#resultPost');
        var $buttonPost = $('#post');

        var $methodGet = $('#methodGet');
        var $textGet = $('#parametersGet');
        var $resultGet = $('#resultGet');
        var $buttonGet = $('#get');

        $buttonPost.on('click',
            function () {

                $.ajax({
                        url: $url.val() + $methodPost.val(),
                        headers: {
                            'token': $token.val(),
                            'user': $user.val()
                        },
                        type: 'POST',
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
(function ($) {

    $('.modal-message-before-submit-sm').on('show.bs.modal', function (event) {
        //values
        var REMOTE = "remote";
        var $button = $(event.relatedTarget);
        var $form = $button.parents("form:first");
        var $title = $button.data('modal-title');
        var $message = $button.data('modal-message');
        var $ok = $button.data('modal-ok');
        var $cancel = $button.data('modal-cancel');
        var $url = $button.data('modal-url');
        var $actionUrl = $button.data('modal-action-url');
        var $modalType = $button.data('modal-type');
        var $modalMode = $button.data('modal-mode');

        //controls
        var $modal = $(this);

        var modalFunction = function () {
            $modal.find('.modal-title').text($title);
            $modal.find('.modal-body').html($message);
            if ($cancel) {
                $modal.find('.modal-cancel').text($cancel);
            }

            if ($modalType === 'info') {
                $modal.find('.modal-header .icon').addClass("text text-info glyphicon-info-sign");
            } else if ($modalType === 'exclamation') {
                $modal.find('.modal-header .icon').addClass("text text-warning glyphicon-exclamation-sign");
            } else if ($modalType === 'error') {
                $modal.find('.modal-header .icon').addClass("text text-danger glyphicon-remove-sign");
            } else {
                $modal.find('.modal-header .icon').addClass("text text-warning glyphicon-question-sign");
            }

            var $buttonOk = $modal.find('.modal-ok');
            $buttonOk.text($ok);
            $buttonOk.on("click", function () {
                if ($actionUrl !== "" && $actionUrl !== undefined && $actionUrl !== null)
                    $form.attr('action', $actionUrl);
                $form.submit();
            });
        };
        var cancelModal = false;
        if ($modalMode === REMOTE) {
            if ($form.valid()) {

                $.ajax({
                    method: "POST",
                    url: $url,
                    data: $form.serialize(),
                    async: false,
                    cache: false,
                })
                    .done(function (data) {
                        if (data.Status) {
                            if (data.Info.ShowMessage) {
                                $title = data.Info.ModalTitle ? data.Info.ModalTitle : $title;
                                $message = data.Info.ModalMessage ? data.Info.ModalMessage : $message;
                                modalFunction();
                            } else {//if showMessage
                                cancelModal = true;
                                $form.submit();
                            }
                        } else {//if modelValid
                            console.log("ErrorMessage:" + data.ErrorMessage);
                            cancelModal = true;
                        }
                    })//ajax
                    .fail(function () {
                        console.log("ErrorMessage: Error in post");
                        cancelModal = true;
                    });// ajax

            } else {//if valid
                cancelModal = true;
            }
        } else {//if remote
            modalFunction();
        }
        if (cancelModal) {
            event.stopImmediatePropagation();
            return false;
        }
    });

    $('.modal-form-submit-sm').on('show.bs.modal', function (event) {
        var $button = $(event.relatedTarget);
        var $url = $button.data('modal-url');
        var $title = $button.data('modal-title');

        var $modal = $(this);
        var cancelModal = false;

        $modal.find('.modal-title').text($title);

        $.ajax({
            method: "GET",
            url: $url,
            async: false,
            cache: false
        })
            .done(function (response) {
                var $modalBody = $modal.find(".modal-body");
                $modalBody.html(response);
                $.validator.unobtrusive.parseDynamicContent($modalBody, true);


            }) //ajax
            .fail(function () {
                console.log("ErrorMessage: Error in post");
                cancelModal = true;
            }); // ajax


        if (cancelModal) {
            event.stopImmediatePropagation();
            return false;
        }

    });



})(jQuery);
(function () {
    $(document).ready(function () {
        $(".order-details").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.order-details-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row",
            addButtonEvents: {
                beforeAdd: function () { },
                afterAdd: function($newRow) {
                    var $commentRequired = $newRow.find("[name*='CommentRequired']");
                    $commentRequired.val(false);
                },
            },
        });
        $(".order-details").each(function(idx, obj) {
            var $itemsContainer = $(obj);
            var $controlId = $("#Id");
            var $inputs = $itemsContainer.find("input[name*='.Amount']");
            var $popoverCommentsTemplate = _.template($('#popoverCommentsTemplate').html());
            if ($controlId.val() > 0) {
                $inputs.each(function() {
                    var $that = $(this);
                    var $container = $that.parents(".order-details-item:first");
                    var $previousAmount = $container.find("[name*='PreviousAmount']");
                    var $commentRequired = $container.find("[name*='CommentRequired']");

                    $that.on('change',
                        function(ev) {
                            if ($previousAmount.val() !== $that.val()) {
                                $commentRequired.val(true);
                            } else {
                                $commentRequired.val(false);
                            }
                        });
                });

                $inputs.addButtonInput({
                    buttonIcon: "glyphicon-th-list",
                    classes: "comments-btn",
                    data: {},
                    click: function($input, $button, additionalData) {
                        $("button.comments-btn").not($button).popover("hide");
                        var $container = $input.parents(".order-details-item:first");
                        var $controlOrderItemId = $container.find("[name*='Id']");
                        var $popoverContainer = $input.parents(".row:first");
                        $.get($itemsContainer.data("comments-url"),
                                { id: $controlId.val(), orderItemId: $controlOrderItemId.val() })
                            .done(function(data) {
                                $button.popover({
                                    html: true,
                                    container: $popoverContainer,
                                    trigger: 'focus',
                                    content: $popoverCommentsTemplate({ comments: data.Info }),
                                    title: $itemsContainer.data("comments-title"),
                                    template:
                                        '<div class="comments-popover popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>',
                                }).on('show.bs.popover',
                                    function() {

                                    }).on('shown.bs.popover',
                                    function(a) {


                                    }).popover('toggle');
                            }).fail(function() {
                                console.log("Comments error");
                            })
                            .always(function() {

                            });
                    }
                });
                $("span.field-validation-valid, span.field-validation-error").addClass('help-block');
            }
        });
    });
})();
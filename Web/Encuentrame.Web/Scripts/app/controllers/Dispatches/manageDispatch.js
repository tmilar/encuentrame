(function () {
    $(document).ready(function () {
        var editableList = function () {
            $(".dispatch-items").makeEditableList({
                containerItemsSelector: "",
                itemSelector: ".row.dispatch-items-item",
                showRemoveButton: true,
                showAddButton: false,
                removeButtonSelector: ".remove-row"
            });
        }
        editableList();
        $(".dispatch-items").each(function (idx, obj) {
            var $itemsContainer = $(obj);
            var $controlRelatedOrder = $("#RelatedOrder");
            var $controlId = $("#Id");

            //$controlRelatedOrder.data("isFirstTime", true);
            var $form = $itemsContainer.parents('form:first');

            var change = function (ev) {
                var $that = $(this);
                // if ($controlRelatedOrder.data("isFirstTime")) {
                //     $controlRelatedOrder.data("isFirstTime", false);
                //      return;
                //  }
                if ($controlRelatedOrder.val()) {
                    $that.disabled();
                    $.get($itemsContainer.data("url"), { id: $controlId.val(), orderId: $controlRelatedOrder.val() })
                        .done(function (data) {
                            $itemsContainer.html(data);
                            configureThisControls($itemsContainer);
                            $.validator.unobtrusive.parseDynamicContent($itemsContainer);
                            editableList();

                            var $inputs = $itemsContainer.find("input[name*='.Amount']");
                            if ($controlId.val() > 0) {
                                var $popoverCommentsTemplate = _.template($('#popoverCommentsTemplate').html());
                                
                                $inputs.each(function() {
                                    var $that = $(this);
                                    var $container = $that.parents(".dispatch-items-item:first");
                                    var $pending = $container.find("[name*='PendingOrderedAmount']");
                                    var $commentRequired = $container.find("[name*='CommentRequired']");
                                    var $actionForDifferenceDispatched =
                                        $container.find("[name*='ActionForDifferenceDispatched']");
                                    $actionForDifferenceDispatched.disabled();
                                    $that.on('change',
                                        function(ev) {
                                            if ($pending.val() !== $that.val()) {
                                                $commentRequired.val(true);
                                                $actionForDifferenceDispatched.enabled();
                                            } else {
                                                $commentRequired.val(false);
                                                $actionForDifferenceDispatched.selectResetToFirstValue();
                                                $actionForDifferenceDispatched.disabled();
                                            }
                                        });
                                });
                                $inputs.addButtonInput({
                                    buttonIcon: "glyphicon-th-list",
                                    classes: "comments-btn",
                                    data: {},
                                    click: function($input, $button, additionalData) {
                                        $("button.comments-btn").not($button).popover("hide");
                                        var $container = $input.parents(".dispatch-items-item:first");
                                        var $controlDispachItemId = $container.find("[name*='Id']");
                                        var $popoverContainer = $input.parents(".row:first");
                                        $.get($itemsContainer.data("comments-url"),
                                                { id: $controlId.val(), dispachItemId: $controlDispachItemId.val() })
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
                            } else {
                               
                                $inputs.each(function () {
                                    var $that = $(this);
                                    var $container = $that.parents(".dispatch-items-item:first");
                                    var $pending = $container.find("[name*='PendingOrderedAmount']");
                                    var $commentRequired = $container.find("[name*='CommentRequired']");
                                    var $actionForDifferenceDispatched =
                                        $container.find("[name*='ActionForDifferenceDispatched']");
                                    $actionForDifferenceDispatched.disabled();
                                    $that.on('change',
                                        function (ev) {
                                            if ($pending.val() !== $that.val()) {
                                                $commentRequired.val(true);
                                                $actionForDifferenceDispatched.enabled();
                                            } else {
                                                $commentRequired.val(false);
                                                $actionForDifferenceDispatched.selectResetToFirstValue();
                                                $actionForDifferenceDispatched.disabled();
                                            }
                                        });
                                });
                                $("span.field-validation-valid, span.field-validation-error").addClass('help-block');
                            }
                        })
                        .fail(function () {
                            $itemsContainer.html("");
                        })
                        .always(function () {
                            $that.enabled();

                        });
                } else {
                    $itemsContainer.html("");
                }
            };

            $controlRelatedOrder.on('change', change);

        });




    });
})();
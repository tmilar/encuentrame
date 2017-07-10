
(function ($) {
    /**
     * Common functions support
     */
    String.prototype.pascalCase = function () {
        return this.charAt(0).toLowerCase() + this.slice(1);
    };

    $.fn.extend({
        updateNamesAndIds: function (options) {
            options = $.extend({}, $.updateNamesAndIds.defaults, options);
            this.each(function () {
                new $.updateNamesAndIds($(this), options);
            });
            return this;
        },

        hasAttr: function (name) {
            this.each(function () {
                new $.hasAttr($(this), name);
            });
            return this;
        },
        loading: function (showLoading) {
            this.each(function () {
                new $.loading($(this), showLoading);
            });
            return this;
        },
        disabled: function () {
            this.each(function () {
                new $.disabled($(this));
            });
            return this;
        },
        enabled: function () {
            this.each(function () {
                new $.enabled($(this));
            });
            return this;
        },
        reIndexId: function (attrName, newIndex) {
            this.each(function () {
                new $.reIndexId($(this), attrName, newIndex);
            });
            return this;
        },
        reIndexName: function (attrName, newIndex) {
            this.each(function () {
                new $.reIndexName($(this), attrName, newIndex);
            });
            return this;
        },
        findRelatedControl: function (id) {
               return new $.findRelatedControl($(this), id);
        },

    });

    $.findRelatedControl = function ($ctl, id) {
        if (!id) {
            id = $ctl.data('related-to');
        }
        var $selectRelated = $('#' + id);
        if ($selectRelated.length === 0) {
            $selectRelated = $('#' + id + '_Id');
            if ($selectRelated.length === 0) {
                var idStr = $ctl.attr("id");
                var prefix = idStr.substring(0, idStr.indexOf('__'));
                $selectRelated = $('#' + prefix + "__" + id);
                if ($selectRelated.length === 0) {
                    $selectRelated = $('#' + prefix + "__" + id + '_Id');
                }
            }
        }
        return $selectRelated;
    };

    $.reIndexName = function ($ctl, attrName, newIndex) {
        if ($ctl.hasAttr(attrName)) {
            var attr = $ctl.attr(attrName);
            if (attr != undefined) {
                $ctl.attr(attrName, attr.replace(new RegExp("[.[0-9+]*]", 'i'), '[' + newIndex + ']'));
            } else {
                console.log("Attr: "+ attrName + " __ ctrl: " + $ctl.attr('id'));
            }
        }
    };
    $.reIndexId = function ($ctl, attrName, newIndex) {
        if ($ctl.hasAttr(attrName)) {
            var attr = $ctl.attr(attrName);
            if (attr != undefined) {
                $ctl.attr(attrName, attr.replace(new RegExp('_.[0-9+]*__', 'i'), '_' + newIndex + '__'));
            }
        }
    };

    $.hasAttr = function ($ctl, name) {
        return $ctl.attr(name) !== undefined;
    };

    $.loading = function ($ctl, showLoading) {
        if (showLoading) {
            $ctl.addClass("loading");
        } else {
            $ctl.removeClass("loading");
        }
    };

    $.disabled = function ($ctl) {
        $ctl.prop("disabled", true);
    };
    $.enabled = function ($ctl) {
        $ctl.prop("disabled", false);
    };
    // list is the element, options is the set of defaults + user options
    $.updateNamesAndIds = function (list, options) {

        $.each(list.find(options.rowSelector), function (index, value) {
            var inputs = $(value).find("input");
            $.each(inputs, function (index2, input) {
                $(input).reIndexName('name', index);
                $(input).reIndexId('id', index);
            });
            var selects = $(value).find("select");
            $.each(selects, function (index22, select) {
                $(select).reIndexName('name', index);
                $(select).reIndexId('id', index);
            });
            var labels = $(value).find("label");
            $.each(labels, function (index3, label) {
                $(label).reIndexId('for', index);
            });
            var spanHelpBlocks = $(value).find("span.help-block");
            $.each(spanHelpBlocks, function (index4, span) {
                $(span).reIndexName('data-valmsg-for', index);
            });
            var spanHelpBlocksMessage = $(value).find("span.help-block span");
            $.each(spanHelpBlocksMessage, function (index5, span) {
                $(span).reIndexId('for', index);
            });
        });
    };

    $.updateNamesAndIds.defaults = {
        rowSelector: '.row'
    };


    /**
     * makeDisabledGroup control
     */

    $.fn.extend({
        makeDisabledGroup: function (options) {
            options = $.extend({}, $.makeDisabledGroup.defaults, options);
            this.each(function () {
                new $.makeDisabledGroup($(this), options);
            });
            return this;
        }
    });

    $.makeDisabledGroup = function ($ctrl, options) {
        var $source = $ctrl.find(options.sourceSelector);
        var $target = $ctrl.find(options.targetSelector);
        $source.change(function () {
            var $that = $(this);
            if ($that.is(':checkbox')) {
                if ($that.is(':checked') === options.sourceValueToDisabled) {
                    $target.prop("disabled", true); // disable all 
                    $target.val("");

                } else {
                    $target.prop("disabled", false); // Enable all
                }
            } else {
                console.log("not implemented!");
            }
        });
        $source.trigger("change");
    }

    $.makeDisabledGroup.defaults = {
        sourceSelector: "input[type='checkbox']",
        sourceValueToDisabled: true,
        targetSelector: "input[type!='checkbox']",
    };
    /**
     * makeEditableList control
     */
    $.fn.extend({
        makeEditableList: function (options) {
            options = $.extend({}, $.makeEditableList.defaults, options);
            this.each(function () {
                new $.makeEditableList($(this), options);
            });
            return this;
        }
    });
    $.makeEditableList = function ($ctrl, options) {
        var $addButton = $ctrl.find(options.addButtonSelector);
        var templates = $ctrl.find(options.itemSelector + ":first");
        var isRequired = ($ctrl.data('mode') === 'required');
        console.log("es requerida: " + (isRequired ? 'SI' : 'NO'));
        if (templates.length <= 0) {
            return;
        }
        var template = templates[0].outerHTML;
        var $container = $ctrl;
        if (options.containerItemsSelector !== "") {
            $container = $ctrl.find(options.containerItemsSelector);
        }

        var specialControlSetUp = function ($row) {
            /*SELECT2*/
            $row.find("span.select2").remove();
            $row.find("select").removeClass("select2-hidden-accessible");
            /*********/

            /*CHECKBOX*/
            $row.find("input:checkbox").each(function (idx, checkbox) {
                var $checkbox = $(checkbox);
                var name = $checkbox.attr("name");
                var $inputs = $row.find("input[name='" + name + "']");

                $checkbox.on('change', function (e) {
                    $inputs.val($checkbox.is(':checked'));
                    $checkbox.val($checkbox.is(':checked'));
                });

            });
            /*********/
        };

        var cleanRow = function ($row) {
            $row.find("input").val("");
            $row.find("input:hidden").val("0");
            $row.find("select").val(null).trigger('change.select2');
            $row.find(":checkbox").prop('checked', false);

            $row.find("input:checkbox").each(function (idx, checkbox) {
                var $checkbox = $(checkbox);
                var name = $checkbox.attr("name");
                var $inputs = $row.find("input[name='" + name + "']");
                $inputs.val(false);
                $checkbox.val(false);
            });

        };
        var removeAction = function (ev) {
            var $that = $(this);
            var $parent = $that.parents(options.itemSelector + ":first");
            if (!options.removeButtonEvents.beforeRemove($parent)) {
                return;
            }
            if ($ctrl.find(options.itemSelector).length === 1 && isRequired) {
                cleanRow($parent);
            } else {
                $parent.remove();
                $ctrl.updateNamesAndIds({ rowSelector: options.itemSelector });
            }
            options.removeButtonEvents.afterRemove();
        }

        var upAction = function (ev) {
            var $that = $(this);
            var $item = $that.parents(options.itemSelector + ":first");

            var $previous = $item.prev();
            if ($previous.length === 0)
                return;

            $item.insertBefore($previous);
            $ctrl.updateNamesAndIds({ rowSelector: options.itemSelector });
        }

        var downAction = function (ev) {
            var $that = $(this);
            var $item = $that.parents(options.itemSelector + ":first");
            var $next = $item.next();
            if ($next.length === 0)
                return;
            $item.insertAfter($next);
            $ctrl.updateNamesAndIds({ rowSelector: options.itemSelector });
        }

        var addAction = function (ev) {

            options.addButtonEvents.beforeAdd();
            var $that = $(this);
            var $newRow = $(template);
            specialControlSetUp($newRow);
            $newRow.appendTo($container);
            cleanRow($newRow);
            $ctrl.updateNamesAndIds({ rowSelector: options.itemSelector });
            configureThisControls($newRow);
            $.validator.unobtrusive.parseDynamicContent($newRow);
            $newRow.find(options.removeButtonSelector).click(removeAction);
            $that.appendTo($container);
            options.addButtonEvents.afterAdd($newRow);
        }

        $addButton.click(addAction);
        $ctrl.find(options.removeButtonSelector).click(removeAction);
        $ctrl.find(options.upButtonSelector).click(upAction);
        $ctrl.find(options.downButtonSelector).click(downAction);
    };

    $.makeEditableList.defaults = {
        containerItemsSelector: "",
        itemSelector: ".row",
        showRemoveButton: true,
        showAddButton: true,
        addButtonSelector: ".add-row",
        addButtonEvents: {
            beforeAdd: function () { },
            afterAdd: function ($newRow) { },
        },
        removeButtonSelector: ".remove-row",
        removeButtonEvents: {
            beforeRemove: function ($row) { return true; },
            afterRemove: function () { },
        },
        upButtonSelector: ".up-row",
        downButtonSelector: ".down-row"

    };

    /**
    * addButtonInput control
    */

    $.fn.extend({
        addButtonInput: function (options) {
            options = $.extend({}, $.addButtonInput.defaults, options);
            this.each(function () {
                new $.addButtonInput($(this), options);
            });
            return this;
        }
    });

    $.addButtonInput = function ($inputs, options) {
        $inputs.each(function (idx, input) {
            var $input = $(input);
            var $inputGroup = $("<div>", { "class": "input-group" });
            var $span = $("<span>", { "class": "input-group-btn" });
            var $button = $("<button>", { "class": "btn btn-default " + options.classes, type: "button" });
            var $buttonSpan = $("<span>", { "class": "glyphicon " + options.buttonIcon });

            if (options.alignIcon !== "right") {

                if (options.buttonIcon !== "") {
                    $button.append($buttonSpan);
                }
                if (options.text !== "") {
                    $button.append(options.text);
                }
            } else {
                if (options.text !== "") {
                    $button.append(options.text);
                }
                if (options.buttonIcon !== "") {
                    $button.append($buttonSpan);
                }
            }
            $span.append($button);

            $inputGroup.insertBefore($input);
            $inputGroup.append($input);
            $inputGroup.append($span);

            $button.on("click",
                function () {
                    options.click($input, $button, options.data);
                });
        });
    }

    $.addButtonInput.defaults = {
        text: "",
        buttonIcon: "",
        classes: "",
        alignIcon: "left",
        data: {},
        click: function ($input) {
            console.log("click on button id: " + $input.attr("id"));
        },
    };

})(jQuery);
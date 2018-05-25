(function ($) {
    /**
     * ARATIVA bootstrap treeview
     */
    'use strict';
    var pluginName = 'treeview';
    var defaults = {};

    defaults.options = {
        selectedIds: '',
        showCheckbox: false,
        textClass: 'text-primary',
        expandIcon: 'glyphicon-plus',
        contractIcon: 'glyphicon-minus',
        onNodeChecked: function (node) { },
        onNodeUnchecked: function (node) { },

    }


    var Treeview = function ($element, options) {
        this.$element = $element;
        this.elementId = $element.prop('id');
        this.options = options;

        this.init();
        this.subscribeEvents();
        this.render();

        return {
            options: this.options,
            getSelectedIds: $.proxy(this.getSelectedIds, this)
        }
    }

    Treeview.prototype.init = function () {
        console.log("Init");
        this.selectedIds = [];
        if (typeof (this.options.selectedIds) === 'string' && this.options.selectedIds !== '') {
            this.selectedIds = this.options.selectedIds.split(',');
        }
    }

    Treeview.prototype.getSelectedIds = function () {
        var that = this;
        var ids = [];
        that.$element.find(':checked').each(function (idx, chk) {
            var $chk = $(chk);
            ids.push($chk.val());
        });
        that.selectedIds = ids;
        return ids.join();
    }

    Treeview.prototype.render = function () {
        console.log("Render");
        var that = this;
        var $nodeGroupContainer = that.nodeGroupContainer().addClass('tree');
        var level = 1;
        $.each(this.options.data, function (idx, nd) {
            $nodeGroupContainer.append(that.buildNode(nd, level));
        });

        $nodeGroupContainer.find('.tree-leaf :checked').each(function(idx, obj) {
            var $obj = $(obj);
            $obj.triggerHandler('click');
        });
        $nodeGroupContainer.find(':checkbox').change(function () {
            var $this = $(this);
            var node = $this.parents('.tree-node:first').data('node');
            if ($this.is(':checked')) {
                that.$element.trigger('nodeChecked', $.extend(true, {}, node));
            } else {
                that.$element.trigger('nodeUnchecked', $.extend(true, {}, node));
            }
        });
        that.$element.append($nodeGroupContainer);
    }

    Treeview.prototype.unsubscribeEvents = function () {
        this.$element.off('nodeChecked');
        this.$element.off('nodeUnchecked');
    }

    Treeview.prototype.subscribeEvents = function () {

        this.unsubscribeEvents();

        if (typeof (this.options.onNodeChecked) === 'function') {
            this.$element.on('nodeChecked', this.options.onNodeChecked);
        }
        if (typeof (this.options.onNodeUnchecked) === 'function') {
            this.$element.on('nodeUnchecked', this.options.onNodeUnchecked);
        }
    }


    Treeview.prototype.nodeGroupContainer = function () {
        return $('<ul>').addClass("list-group tree-node-group");
    }

    Treeview.prototype.nodeContainer = function () {
        return $('<li>').addClass("list-group-item tree-node");
    }

    Treeview.prototype.expandIcon = function () {
        var that = this;
        var $expand = $("<span>");
        $expand.addClass("icon icon-expand").prop('aria-hidden', 'true').addClass('glyphicon ' + that.options.contractIcon);
        $expand.click(function () {
            var $this = $(this);
            $this.toggleClass(that.options.contractIcon + ' ' + that.options.expandIcon);
            var $childsNodes = $this.parents('.tree-node:first').find(".tree-node-group:first");
            if ($childsNodes.is(":visible")) {
                $childsNodes.hide();
            } else {
                $childsNodes.show();
            }
        });
        return $expand;
    }


    Treeview.prototype.checkboxParentState = function ($node) {
        var $parent = $node.parents('.tree-node:first');
        if ($parent.length > 0) {
            var $childsContainer = $parent.find('.tree-node-group:first');
            var checkboxCount = $childsContainer.find(':checkbox').length;
            var checkedCount = $childsContainer.find(":checked").length;
            if (checkboxCount !== checkedCount && checkedCount > 0) {
                $parent.find(":checkbox:first").prop("indeterminate", true).prop("checked", false);
            } else if (checkboxCount === checkedCount) {
                $parent.find(":checkbox:first").prop("indeterminate", false).prop("checked", true);
            } else {
                $parent.find(":checkbox:first").prop("indeterminate", false).prop("checked", false);
            };
            this.checkboxParentState($parent);
        }
    }

    Treeview.prototype.nodeParentDisplay = function ($node) {
        var $parent = $node.parents('.tree-node:first');
        if ($parent.length > 0) {
            var $childsContainer = $parent.find('.tree-node-group:first');
            var checkboxCount = $childsContainer.find(':checkbox').length;
            var checkedCount = $childsContainer.find(":checked").length;
            if (checkboxCount !== checkedCount && checkedCount > 0) {
                $parent.find(":checkbox:first").prop("indeterminate", true).prop("checked", false);
            } else if (checkboxCount === checkedCount) {
                $parent.find(":checkbox:first").prop("indeterminate", false).prop("checked", true);
            } else {
                $parent.find(":checkbox:first").prop("indeterminate", false).prop("checked", false);
            };
            this.checkboxParentState($parent);
        }
    }
    Treeview.prototype.checkbox = function (node) {
        var that = this;
        var $checkbox = $('<input />', { type: 'checkbox', value: node.value });
        if (that.selectedIds.length > 0) {            
            $checkbox.prop("checked", $.inArray(node.value.toString(), that.selectedIds)>=0);
        } else {
            $checkbox.prop("checked", node.checked);
        }
        $checkbox.click(function () {
            var $this = $(this);
            var $node = $this.parents('.tree-node:first');
            $node.find(".tree-node-group").find(":checkbox").prop("checked", $this.is(":checked"));
            that.checkboxParentState($node);
        });

        return $checkbox;
    }

    Treeview.prototype.label = function (text) {
        var $label = $('<label />').addClass(this.options.textClass);
        $label.text(text);
        return $label;
    }

    Treeview.prototype.buildNode = function (node, level) {
        var that = this;
        var $checkbox = null;
        var $expandIcon = null;
        var $childNodesContainer = null;
        var $label = null;

        if (this.options.showCheckbox) {
            $checkbox = this.checkbox(node);
        }

        $label = this.label(node.text);

        //Create childs
        if (node.nodes && node.nodes.length>0) {
            $expandIcon = that.expandIcon();
            $childNodesContainer = that.nodeGroupContainer();
            $.each(node.nodes, function (idx, nd) {
                $childNodesContainer.append(that.buildNode(nd, level + 1));
            });
        }

        var $nodeContainer = that.nodeContainer();//Create node
        $nodeContainer.data('node', node);
        $nodeContainer.attr('level', level);
        if ($expandIcon != null) { //expand icon
            $nodeContainer.append($expandIcon);
            $nodeContainer.addClass('tree-branch');
        } else {
            $nodeContainer.addClass('tree-leaf');
        }
        if ($checkbox != null) {//checkbox control
            $nodeContainer.append($checkbox);
        }

        if ($label != null) {
            $nodeContainer.append($label);//text
        }

        if ($childNodesContainer != null) {//childs
            $nodeContainer.append($childNodesContainer);
        }

        return $nodeContainer;
    }


    $.fn[pluginName] = function (options, args) {

        var result;
        this.each(function () {
            var that = $.data(this, pluginName);
            if (typeof options === 'string') {
                if (!that) {
                    console.log('Not initialized, can not call method : ' + options);
                } else if (!$.isFunction(that[options]) || options.charAt(0) === '_') {
                    console.log('No such method : ' + options);
                } else {
                    if (!(args instanceof Array)) {
                        args = [args];
                    }
                    result = that[options].apply(that, args);
                }
            } else {
                options = $.extend({}, defaults.options, options);
                $.data(this, pluginName, new Treeview($(this), options));

            }
        });
        return result;
    };


})(jQuery);
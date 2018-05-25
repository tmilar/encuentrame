var configureTreeviewControls = function configureTreeviewControls($dom) {

    $dom.find('.treeview-editor').each(function (idx, obj) {
        var $checkableTree = $(this);
        var $target = $('#' + $checkableTree.data('selected-ids-target'));
        $checkableTree.treeview({
            data: $checkableTree.data('src'),
            selectedIds: $target.val(),
            showCheckbox: true,
            onNodeChecked: function (ev,node) {
                console.log('checked node: ' + node.text + ' ' + node.value);
                $target.val($checkableTree.treeview('getSelectedIds'));
            },
            onNodeUnchecked: function (ev, node) {
                console.log('unchecked node: ' + node.text + ' ' + node.value);
                $target.val($checkableTree.treeview('getSelectedIds'));
            },
        });
    });

    $dom.find('.treeview-display').each(function (idx, obj) {
        var $displayableTree = $(this);
        
        $displayableTree.treeview({
            data: $displayableTree.data('src'),
            showCheckbox: false,
        });
    });

};
(function () {
    $(document).ready(function () {
        configureTreeviewControls($(document));
    });
})();
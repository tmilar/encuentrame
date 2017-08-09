(function () {
    $(document).ready(function () {
        $(".disabled-group-control").makeDisabledGroup({
            sourceSelector: "input[type=checkbox]",
            sourceValueToDisabled: true,
            targetSelector: "select",
        });
        
        $(".suppliers").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.suppliers-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
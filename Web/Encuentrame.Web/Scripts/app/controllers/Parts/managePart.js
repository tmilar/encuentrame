(function () {
    $(document).ready(function () {
        $(".disabled-group-control").makeDisabledGroup({
            sourceSelector: "input[type=checkbox]",
            sourceValueToDisabled: true,
            targetSelector: "select",
        });

        $(".client-informations").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.client-informations-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".storage-type-informations").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.storage-type-informations-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".part-components").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.part-components-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".part-process").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.part-process-item",
            showRemoveButton: false,
            showAddButton: false,
            addButtonSelector: ".add-row",
            upButtonSelector: ".up-row",
            downButtonSelector: ".down-row"
        });

       
    });
})();
(function () {
    $(document).ready(function () {
        $(".machines").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.machines-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".parts").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.parts-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
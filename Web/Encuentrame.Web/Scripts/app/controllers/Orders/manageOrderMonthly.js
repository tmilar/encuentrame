(function () {
    $(document).ready(function () {
        $(".order-details").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.order-details-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
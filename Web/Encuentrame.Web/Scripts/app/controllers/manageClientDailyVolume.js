(function () {
    $(document).ready(function () {

        $(".part-use-per-units").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.part-use-per-units-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
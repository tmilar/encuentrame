(function () {
    $(document).ready(function () {
        $(".raw-materials-in-factories").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".raw-material-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
(function () {
    $(document).ready(function () {
       
        $(".contacts").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.contacts-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
    });
})();
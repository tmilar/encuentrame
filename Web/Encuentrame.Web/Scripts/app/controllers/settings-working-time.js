(function () {


    $(document).ready(function () {
        $(".special-working-hours").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.special-working-hours-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".no-working-hours").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.no-working-hours-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });
       
    });
})();
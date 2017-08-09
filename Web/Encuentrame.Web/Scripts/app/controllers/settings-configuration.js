(function () {


    $(document).ready(function () {
        $(".raw-material-types").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.raw-material-types-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

      
       
    });
})();
(function () {
    $(document).ready(function () {

        $(".part-audits").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.part-audits-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".part-partially-audits").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.part-partially-audits-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

        $(".raw-material-audits").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".row.raw-material-audits-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row"
        });

    });
})();
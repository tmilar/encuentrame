var configureCreateSection = function ($dom) {  
    $(".order-month-dayly-detail").makeEditableList({
        containerItemsSelector: "",
        itemSelector: ".order-month-dayly-detail-item",
        showRemoveButton: true,
        showAddButton: true,
        addButtonSelector: ".add-row",
        removeButtonSelector: ".remove-row"
    });
};

(function () {
    var $document = $(document);
    $document.ready(function () {
        configureCreateSection($document);
    });
})();
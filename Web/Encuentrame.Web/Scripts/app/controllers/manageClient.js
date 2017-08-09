(function () {
    $(document).ready(function () {
        $(".client-addresses").makeEditableList({
            containerItemsSelector: "",
            itemSelector: ".client-address-item",
            showRemoveButton: true,
            showAddButton: true,
            addButtonSelector: ".add-row",
            removeButtonSelector: ".remove-row",
            addButtonEvents: {
                beforeAdd: function () { },
                afterAdd: function ($newRow) {
                    var $newRowCheckbox = $newRow.find('.default-checkbox .check-box');
                    $newRowCheckbox.prop('checked', false);
                    $newRowCheckbox.click(function () {
                        onCheckboxClick($(this));
                    });
                }
            }
        });

        $(".client-addresses .default-checkbox .check-box").click(function () {
            onCheckboxClick($(this));            
        });

        function onCheckboxClick($currentCheckbox) {           
            var checkedState = $currentCheckbox.is(":checked");
            $currentCheckbox.parents('.client-addresses').find('.default-checkbox .check-box:checked').each(function () {
                $(this).prop('checked', false);
            });
            $currentCheckbox.prop('checked', checkedState);
        }
    });
})();
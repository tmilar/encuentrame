var configureFilterSector = function ($dom) {
    var $stockPerDateSection = $dom.find('.stock-per-date-section');
    var $filterSection = $stockPerDateSection.find('.filter-panel');
    var $datePickers = $filterSection.find('.date-control');
    var $stepUnitSelector = $filterSection.find('.step-unit-selector');
    var $tableSection = $stockPerDateSection.find('.table-section');
    var $filterForm = $filterSection.find('form');
    var currentFormat = "dd/mm/yyyy";

    $stepUnitSelector.on('change', function () {
        var $stepUnitSelect = $(this);
        var selectedValue = $stepUnitSelect.find("option:selected").val();
        var viewMode = "days";

        switch (selectedValue) {
            case "Day":
                viewMode = "days";
                break;
            case "Month":
                viewMode = 1;
                break;
            default:
                viewMode = 2;
        }

        $datePickers.each(function (idx, obj) {
            var $currentDatePicker = $(this);

            $currentDatePicker.datepicker("remove");
            $currentDatePicker.val("");

            $currentDatePicker.datepicker({
                format: currentFormat,
                startDate: "01/01/1920",
                language: "es",
                autoclose: true,
                todayHighlight: true,
                startView: viewMode,
                minViewMode: viewMode
            });
        });
    });

    $filterForm.submit(function () {
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    $tableSection.html(result);
                }
            });
        }
        return false;
    });
};

(function () {
    var $document = $(document);
    $document.ready(function () {
        configureFilterSector($document);
    });
})();
var configureFilterSector = function ($dom) {
    var $monthDailyContainer = $dom.find('.month-daily-container');    
    var $datePickers = $monthDailyContainer.find('.date-control');
  
    var currentFormat = "dd/mm/yyyy";

    var viewMode = 1; //MonthFormat

        $datePickers.each(function (idx, obj) {
            var $currentDatePicker = $(this);

            $currentDatePicker.datepicker("remove");

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

};

(function () {
    var $document = $(document);
    $document.ready(function () {
        configureFilterSector($document);
    });
})();
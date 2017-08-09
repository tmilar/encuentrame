(function () {
    $(document).ready(function () {
        var COLORS = 5;
        var TIMERINTERVAL = 60000;
        var currentDate = moment().format();
        var currentSectorId =null;
        var currentMachineCategoryId = null;
        var currentPartId = null;
        var currentPeriod = 5;
        var fillGridTimer;
        var $previousByPeriod = $(".nav-calendar nav .pager .previous .by-period");
        var $previousByDay = $(".nav-calendar nav .pager .previous .by-day");
        var $nextByPeriod = $(".nav-calendar nav .pager .next .by-period");
        var $nextByDay = $(".nav-calendar nav .pager .next .by-day");
        var $todayByDay = $(".nav-calendar nav .pager .today .by-day");

        var $calendarGrid = $('.calendar-grid');
        var $labelGrid = $('.label-grid');
        var $programGrid = $('.program-grid');

        var calendarRow = function () {
            var $div = $("<div>");
            $div.addClass("calendar-row");
            return $div;
        };
        var labelRow = function () {
            var $div = $("<div>");
            $div.addClass("label-row");
            return $div;
        };
        var programRow = function () {
            var $div = $("<div>");
            $div.addClass("program-row");
            return $div;
        };

        var minuteToPercent = function (full, partial) {
            var percent = partial * 100 / full;

            return percent;
        };

        var minuteFromFirtsDay = function (workingDays, dateTime) {
            var minutes = 0;
            if (dateTime < workingDays[0].Begin) {
                return minutes;
            }

            for (var ii = 0; ii < workingDays.length; ii++) {
                var workingDay = workingDays[ii];
                if (dateTime > workingDay.End) {
                    minutes += workingDay.Minutes;
                } else {
                    var partial = moment(dateTime).diff(workingDay.Begin, 'minutes');

                    console.log("Bar datetime: " + moment(dateTime).format("dddd DD/MM HH:mm"));
                    console.log("workingDay datetime: " + moment(workingDay.Begin).format("dddd DD/MM HH:mm"));
                    console.log("Partial: " + partial);
                    if (partial < 0) {
                        partial = 0;
                    }
                    minutes += partial;
                    break;
                }
            }
            return minutes;
        };

        var beginDateTimeIsOutOfTheGrid = function(workingDays, dateTime) {
            return dateTime < workingDays[0].Begin;
        };

        var $dayHeaderTemplate = _.template($('#dayHeaderTemplate').html());
        var $dayTemplate = _.template($('#dayTemplate').html());
        var $labelTemplate = _.template($('#labelTemplate').html());
        var $barTemplate = _.template($('#barTemplate').html());
        var $barPopoverTemplate = _.template($('#barPopoverTemplate').html());
        var $barPopoverTitleTemplate = _.template($('#barPopoverTitleTemplate').html());
        var $form1Template = _.template($('#form1Template').html());
        var $form2Template = _.template($('#form2Template').html());
        var $form3Template = _.template($('#form3Template').html());
        var $form4Template = _.template($('#form4Template').html());

        var pixels = $calendarGrid.width();
        var fillGrid;

        function startTimer() {
            fillGridTimer = setInterval(function () {
                if ($(".bar-popover.popover.in").length > 0) {
                    console.log("dont run fillGrid");
                    return;
                }
                console.log("run fillGrig");
                fillGrid();
            }, TIMERINTERVAL);
        }

        function stopTimer() {
            clearInterval(fillGridTimer);
        }

        fillGrid = function () {
            $.ajax({
                method: "GET",
                url: "weeklyInfo",
                data: { day: currentDate, sectorId: currentSectorId, machineCategoryId: currentMachineCategoryId, partId: currentPartId }
            })
            .done(function (data) {
                if (data.Status) {
                    var gridInfo = data.Info;
                    //fill calendarGrid
                    $calendarGrid.empty();
                    $labelGrid.find('.label-row:not(:first)').remove();
                    $programGrid.empty();
                    var $calendarRow = calendarRow();

                    $calendarGrid.append($calendarRow);
                    $(gridInfo.WorkingDays).each(function(idx, day) {
                        $calendarRow.append($dayHeaderTemplate({
                            caption: moment(day.Begin).format("dddd DD/MM"),
                            begin: moment(day.Begin).format("HH:mm"),
                            end: moment(day.End).format("HH:mm"),
                            width: minuteToPercent(gridInfo.TotalMinutes, day.Minutes),
                        }));
                    });

                    for (var xx = 0; xx < gridInfo.MachineWorks.length; xx++) {
                        var $row = calendarRow();
                        $calendarGrid.append($row);
                        for (var ii = 0; ii < gridInfo.WorkingDays.length; ii++) {
                            $row.append($dayTemplate({
                                width: minuteToPercent(gridInfo.TotalMinutes, gridInfo.WorkingDays[ii].Minutes),
                            }));
                        }
                    }

                    //fill labels
                    $(gridInfo.MachineWorks).each(function(idxm, machine) {
                        var previousProductionOrderIsComplete = true;
                        var $labelRow = labelRow();
                        $labelGrid.append($labelRow);
                        $labelRow.append($labelTemplate({
                            id: machine.MachineId,
                            caption: machine.Caption,
                            isOnline: machine.IsOnline,
                        }));
                        $labelRow.find('[data-toggle="tooltip"]').tooltip();
                        var $programRow = programRow();
                        $programGrid.append($programRow);
                            
                        //fill program
                        $(machine.ProductionOrders).each(function(idxb, bar) {
                            var marginMinutes = minuteFromFirtsDay(gridInfo.WorkingDays, bar.Begin);
                            var widthMinutes = minuteFromFirtsDay(gridInfo.WorkingDays, bar.End) - marginMinutes;
                            var margin = minuteToPercent(gridInfo.TotalMinutes, marginMinutes);
                            var width = minuteToPercent(gridInfo.TotalMinutes, widthMinutes);
                            var setupWidth = 0;
                            if (!beginDateTimeIsOutOfTheGrid(gridInfo.WorkingDays, bar.Begin)) {
                                setupWidth = minuteToPercent(widthMinutes, bar.SetupMinutes);
                            }
                            var objectToTemplate = {
                                id: bar.ProductionOrderId,
                                caption: bar.Caption,
                                status: bar.Status,
                                margin: margin,
                                width: (width + margin) > 100 ? 100 - margin : width,
                                setupWidth: setupWidth,
                                begin: moment(bar.Begin).format("DD/MM HH:mm"),
                                end: moment(bar.End).format("DD/MM HH:mm"),
                                amount: bar.Amount,
                                completedQuantity: bar.CompletedQuantity,
                                color: bar.Status , /*(idxm % COLORS) + 1,*/
                                previousProductionOrderIsComplete: previousProductionOrderIsComplete,
                                quantityPart: []
                            };
                            $.each(bar.WorkDoneQuantityParts, function (idx, data) {
                                objectToTemplate.quantityPart.push({
                                    partId: data.PartId,
                                    partName: data.PartName,
                                    completedQuantity: data.CompletedQuantity,
                                    rejectedQuantity: data.RejectedQuantity,
                                });
                            });
                           

                            var barCompiled = $barTemplate(objectToTemplate);
                            var $bar = $(barCompiled);
                            $programRow.append($bar);
                            $bar.data("click-popover", previousProductionOrderIsComplete);
                            if (bar.Status !== 30) {//30 is completed state
                                previousProductionOrderIsComplete = false;
                            }
                            $bar.on('click', function() {
                                var $me = $(this);
                                $me.off('click');
                                $me.popover({
                                    html: true,
                                    content: $barPopoverTemplate(objectToTemplate),
                                    placement: 'bottom',
                                    title: $barPopoverTitleTemplate(objectToTemplate),
                                    template: '<div class="bar-popover popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>',
                                }).on('show.bs.popover', function() {
                                    $('.bar').not(this).popover('hide');
                                }).on('shown.bs.popover', function() {
                                    var $form = $('#barPopover_' + bar.ProductionOrderId);
                                    var $popover = $form.parents('.bar-popover:first');
                                    var $popoverFormControls = $popover.find('.popover-form-controls:first');
                                    var $popoverFormFields = $popoverFormControls.find('.popover-form-fields:first');
                                    var $popoverButtons = $popover.find('.popover-buttons:first');
                                    
                                    $popover.find('button.button-move').on('click', function () {
                                        var $button = $(this);
                                        $.ajax({
                                            type: 'POST',
                                            url: $button.data('url'),
                                            data: { ProductionOrderId: bar.ProductionOrderId, Direction: $button.data('direction') },
                                            cache: false,
                                        }).done(function (data) {
                                            if (data.Status) {
                                                $('.bar').popover('hide');
                                                fillGrid();
                                            } else {
                                                console.log("Error: " + data.ErrorMessage);
                                            }
                                        }).fail(function (data) {
                                            console.log("Error: " + JSON.stringify(data));
                                        });
                                        
                                    });


                                    $form.find('button.button-ok').on('click', function() {
                                        if ($form.valid()) {
                                            $.ajax({
                                                type: $form.attr('method'),
                                                url: $form.attr('action'),
                                                data: $form.serialize(),
                                                cache: false,
                                            }).done(function(data) {
                                                if (data.Status) {
                                                    $('.bar').popover('hide');
                                                    fillGrid();
                                                } else {
                                                    console.log("Error: " + data.ErrorMessage);
                                                }
                                            }).fail(function(data) {
                                                console.log("Error: " + JSON.stringify(data));
                                            });
                                        }
                                    });

                                    $form.find('button.button-goback').on('click', function() {
                                        $popover.css({ "width": $popover.width() + "px" });
                                        $popoverFormControls.slideUp("fast", function() {
                                            $popoverButtons.show("fast", function() {
                                                $popover.css({ width: '' });
                                                $popoverFormFields.html('');
                                            });
                                        });
                                    });

                                    $popover.find('button').not('.button-move').not('.button-ok').not('.button-goback').on('click', function () {
                                        var formTemplate = $(this).data('form');
                                        if (formTemplate === 'form1') {
                                            $popoverFormFields.html($form1Template({}));
                                        } else if (formTemplate === 'form2') {
                                            $popoverFormFields.html($form2Template({}));
                                        } else if (formTemplate === 'form3') {
                                            $popoverFormFields.html($form3Template(objectToTemplate));
                                        } else if (formTemplate === 'form4') {
                                            $popoverFormFields.html($form4Template(objectToTemplate));
                                        }

                                        var listOptions = {
                                            containerItemsSelector: "",
                                            itemSelector: ".row.work-done-quantity-part",
                                            showRemoveButton: false,
                                            showAddButton: false,
                                            addButtonSelector:'.add-button',
                                        };
                                        $popoverFormFields.append($("<div>").addClass('add-button'));
                                        $popoverFormFields.makeEditableList(listOptions);
                                        $.each(objectToTemplate.quantityPart, function (idx, data) {
                                            if (idx !== 0) {
                                                $popoverFormFields.find(".add-button").trigger('click');
                                            }
                                            var $lastRow = $popoverFormFields.find(".row.work-done-quantity-part:last");

                                            $lastRow.find("[name*='PartId']").val(data.partId);
                                            $lastRow.find("[name*='PartName']").text(data.partName);
                                            $lastRow.find("[name*='CompletedQuantity']").val(data.completedQuantity);
                                            $lastRow.find("[name*='RejectedQuantity']").val(data.rejectedQuantity);
                                        });

                                        $popoverFormFields.find(".add-button").remove();
                                        $form.find('.form-group').addClass("form-group-sm");
                                        $form.find("[name='DateTime']").val(moment().format('DD/MM/YYYY HH:mm'));
                                        $form.find("[name='ProductionOrderId']").val(bar.ProductionOrderId);
                                        $form.find("[name='NewStatus']").val($(this).data('new-status'));

                                        $popover.css({ "width": $popover.width() + "px" });
                                        $popoverButtons.slideUp("fast", function() {
                                            $popoverFormControls.show("fast", function() {
                                                $popover.css({ width: '' });
                                                configureThisControls($form);
                                                $.validator.unobtrusive.parseDynamicContent($form);
                                            });
                                        });
                                    });
                                });
                                $(this).popover('show');

                            });
                        });
                    });
                } else { //if Status
                    console.log("Error: " + data.ErrorMessage);
                }
            });
        };//fillGrid





        $previousByPeriod.on('click', function () {
            currentDate = moment(currentDate).subtract(currentPeriod, "days").format();
            fillGrid();
        });

        $previousByDay.on('click', function () {
            currentDate = moment(currentDate).subtract(1, "days").format();
            fillGrid();
        });

        $nextByPeriod.on('click', function () {
            currentDate = moment(currentDate).add(currentPeriod, "days").format();
            fillGrid();
        });

        $nextByDay.on('click', function () {
            currentDate = moment(currentDate).add(1, "days").format();
            fillGrid();
        });

        $todayByDay.on('click', function () {
            currentDate = moment().format();
            fillGrid();
        });
        $('#Sector').on('change', function () {
            var $this = $(this);
            currentSectorId = $this.val();
            fillGrid();
        });
        $('#MachineCategory').on('change', function () {
            var $this = $(this);
            currentMachineCategoryId = $this.val();
            fillGrid();
        });

        $('#Part').on('change', function () {
            var $this = $(this);
            currentPartId = $this.val();
            fillGrid();
        });
        fillGrid();
        startTimer();
    });
})();
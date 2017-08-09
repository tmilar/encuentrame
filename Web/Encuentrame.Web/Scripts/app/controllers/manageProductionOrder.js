(function () {
    $(document).ready(function () {
        $(".disabled-group-control").makeDisabledGroup({
            sourceSelector: "input[type=checkbox]",
            sourceValueToDisabled: true,
            targetSelector: ".datetime-control",
        });

        $(".operation-details-control").each(function (idx, obj) {
            var $controlContainer = $(obj);
            var $control = $controlContainer.find("select");
            var $viewer = $($controlContainer.data("viewer"));
            
            var change = function(ev) {
                var selectedValue = $control.val();
                if (selectedValue !== "" && selectedValue!=null) {
                    
                    $control.disabled();
                    $viewer.find(".alert").loading(true);
                    $.get($controlContainer.data("url"), { operationId: selectedValue })
                        .done(function(data) {
                            $viewer.html(data);
                        })
                        .fail(function() {
                            $viewer.html("");
                        })
                        .always(function() {
                            $control.enabled();
                            $viewer.find(".alert").loading(false);
                        });
                } else {
                    $viewer.html("");
                }
            };
            $control.on('change',change);
            change();
        });
        
        $(".machine-selector-viewer").each(function (idx, obj) {
            var $viewer = $(obj);
            var $controlMachine = $("#MachineId");
            var $controlOperation = $("#Operation");
            var $controlAmount = $("#PlannedQuantity");
            var $controlBeginDateTime = $("#BeginDateTime");
            
            var $controlAddToQueue = $("#AddToQueue");
            var $form = $viewer.parents('form:first');

            var change = function (ev) {
                var $that = $(this);
                $controlMachine.val('');
               
                if ($controlOperation.val() !== '' && $controlAmount.val() > 0 && ($controlAddToQueue.is(':checked') || $controlBeginDateTime.val() !== '')) {
                    $that.disabled();
                    $viewer.find(".alert").loading(true);
                    $.get($viewer.data("url"), { operationId: $controlOperation.val(), amount: $controlAmount.val(), beginDateTime: $controlBeginDateTime.val() })
                        .done(function (data) {
                            $viewer.html(data);
                            $.validator.unobtrusive.parseDynamicContent($viewer);
                            $viewer.find('input[type=radio][name=MachineIdRadio]').change(function () {
                                var $otherThat = $(this);
                                $controlMachine.val($otherThat.val());
                                $form.valid();
                            });
                        })
                        .fail(function () {
                            $viewer.html("");
                        })
                        .always(function () {
                            $that.enabled();
                            $viewer.find(".alert").loading(false);
                        });
                } else {
                    $viewer.html("");
                }
            };

            $controlOperation.on('change', change);
            $controlAmount.on('change', change);
            $controlAddToQueue.on('change', change);
            $controlBeginDateTime.on('dp.change', change);//dp.change is a bootstrap-datetimepicker's event, is equal to change default event.
            
        });

        $(".raw-material-selector-viewer").each(function (idx, obj) {
            var $viewer = $(obj);
            var $rawMaterialIdsContainer = $(".raw-material-selected-ids-container");

            var $controlOperation = $("#Operation");
            var $controlProductionOrderId = $("#productionOrderId");
            
            var $form = $viewer.parents('form:first');

            var change = function (ev) {

                if ($controlOperation.val() !== '') {
                    $controlOperation.disabled();
                    $viewer.find(".alert").loading(true);
                    $.get($viewer.data("url"), { operationId: $controlOperation.val(), productionOrderId: $controlProductionOrderId.val() })
                        .done(function (data) {
                            $viewer.html(data);
                            var $checkbox = $viewer.find('input[type=checkbox][name=RawMaterialCheckbox]');
                            $checkbox.change(function () {
                                $rawMaterialIdsContainer.empty();
                                var realIdx = 0;
                                $checkbox.each(function (idx, obj) {
                                    var $obj = $(obj);
                                    if ($obj.is(':checked')) {
                                        var $input = $("<input>", { type: "hidden",id: "RawMaterialInFactoryIds_" + realIdx + "_", name: "RawMaterialInFactoryIds[" + realIdx + "]" });
                                        $input.val($obj.val());
                                        $rawMaterialIdsContainer.append($input);
                                        realIdx = realIdx + 1;
                                    }
                                });
                                
                                $form.valid();
                            });
                        })
                        .fail(function () {
                            $viewer.html("");
                        })
                        .always(function () {
                            $controlOperation.enabled();
                            $viewer.find(".alert").loading(false);
                        });
                } else {
                    $viewer.html("");
                }
            };

            $controlOperation.on('change', change);
            
            change();
        });

        $('button.refresh-quantity').on('click', function () {
            var $plannedQuantity = $("#PlannedQuantity");
            var quantities= {};
            $(".raw-material-selector-viewer input:checked").each(function(idx, obj) {
                var $obj = $(obj);
                var quantity = parseFloat($obj.data("quantity"));
                var componentQuantity = parseFloat($obj.data("component-quantity"));
                var rawMaterialId = $obj.data("raw-material-id");
                if (quantities[rawMaterialId] === undefined) {
                    quantities[rawMaterialId] = 0;
                }
                quantities[rawMaterialId] += parseInt(quantity / componentQuantity);
            });

            var quantity = 0;

            for(var propertyName in quantities) {
                var value = quantities[propertyName];
                quantity = (value > quantity) ? value : quantity;
            }
            
            $plannedQuantity.val(quantity);
            $plannedQuantity.focus();
        });
    });
})();
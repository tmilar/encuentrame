(function () {

    $(['requiredif', 'regularexpressionif', 'rangeif']).each(function (index, validationName) {
        $.validator.addMethod(validationName,
                function (value, element, parameters) {
                    // Get the name prefix for the target control, depending on the validated control name
                    var prefix = "";
                    var lastDot = element.name.lastIndexOf('.');
                    if (lastDot != -1) {
                        prefix = element.name.substring(0, lastDot + 1).replace('.', '_');
                    }
                    var id = '#' + prefix + parameters['dependentproperty'];
                    // get the target value
                    var targetvalue = parameters['targetvalue'];
                    targetvalue = (targetvalue == null ? '' : targetvalue).toString();
                    // get the actual value of the target control
                    var $control = $(id);
                    if ($control.length == 0 && prefix.length > 0) {
                        // Target control not found, try without the prefix
                        $control = $('#' + parameters['dependentproperty']);
                    }
                    if ($control.length > 0) {
                        var actualvalue = "";
                        if ($control.is(':checkbox')) {
                            if ($control.prop("indeterminate"))
                                actualvalue = null;
                            else
                                //actualvalue = $(":checkbox:checked[name='" + control.attr("id") + "']").length > 0 ? 'true' : 'false';
                                actualvalue = $control.is(':checked') ? 'true' : 'false';
                        } else if ($control.is('select')) {
                            actualvalue = $('option:selected', $control).val();
                        } else {
                            actualvalue = $control.val();
                        }

                        var operation = parameters['operation'];
                        var conditionVerified = false;
                        switch (operation) {
                            case "RegularExpression":
                                var regexValidation = new RegExp(targetvalue);
                                if (regexValidation.test(actualvalue)) {
                                    conditionVerified = true;
                                }
                                break;
                            case "StartsWith":
                                if (actualvalue.startsWith(targetvalue)) {
                                    conditionVerified = true;
                                }
                                break;
                            case "EndsWith":
                                if (actualvalue.endsWith(targetvalue)) {
                                    conditionVerified = true;
                                }
                                break;
                            case "Contains":
                                if (actualvalue.includes(targetvalue)) {
                                    conditionVerified = true;
                                }
                                break;
                            case "Range":
                                var targetValueParts = targetvalue.split('|');
                                var currentValue = parseFloat(actualvalue, 10);
                                if (targetValueParts.length > 1) {
                                    var min = parseFloat(targetValueParts[0], 10);
                                    var max = parseFloat(targetValueParts[1], 10);
                                    if (isNaN(min) && isNaN(max)) {
                                        conditionVerified = false;
                                    } else if (isNaN(min) && currentValue <= max) {
                                        conditionVerified = true;
                                    } else if (isNaN(max) && currentValue >= min) {
                                        conditionVerified = true;
                                    }
                                    else if (currentValue <= max && currentValue >= min) {
                                        conditionVerified = true;
                                    }                                    
                                }                                
                                break;
                            case "HasAny":
                                if (actualvalue !== undefined && actualvalue !== null && actualvalue !== "")
                                    conditionVerified = true;
                                break;
                            case "Equals":
                            default:
                                if (targetvalue.toLowerCase() === actualvalue.toLowerCase()) {
                                    conditionVerified = true;
                                }
                                break;
                        }

                        var $elementToBeValidated = $(element).parents('.form-group');
                        var rule = parameters['rule'];

                        if (conditionVerified === true) {                            
                            var ruleparam = parameters['ruleparam'];
                            if(rule === 'required')
                                $elementToBeValidated.addClass('form-group-to-validate');
                            
                            return $.validator.methods[rule].call(this, value, element, ruleparam);
                        } else if (rule === 'required') {
                            $elementToBeValidated.removeClass('form-group-to-validate');
                        }
                        // if the condition is true, reuse the existing validator functionality                        
                    }
                    return true;
                }
            );

        $.validator.unobtrusive.adapters.add(validationName, ['dependentproperty', 'targetvalue', 'rule', 'ruleparam', 'operation'], function (options) {
            var rp = options.params['ruleparam'];
            var parameters = {
                dependentproperty: options.params['dependentproperty'],
                targetvalue: options.params['targetvalue'],
                rule: options.params['rule']
                //operation: options.params['operation']
            };

            if (typeof options.params.operation !== "undefined"){
                parameters.operation = options.params['operation'];
            }

            options.rules[validationName] = parameters;

            if (rp) {
                options.rules[validationName].ruleparam = rp.charAt(0) == '[' ? eval(rp) : rp;
            }
            options.messages[validationName] = options.message;
        });
    });

    //data-val-requiredif-rule
    $('[data-val-requiredif-rule]').each(function(idx, obj) {
        var $requiredIfConditionControl = $(this);
        if ($requiredIfConditionControl.data('val-requiredif-rule')) {
            var elementName = $requiredIfConditionControl.attr('name');
            var prefix = "";
            var lastDot = elementName.lastIndexOf('.');
            if (lastDot !== -1) {
                prefix = elementName.substring(0, lastDot + 1).replace('.', '_');
            }

            //data-val-requiredif-dependentproperty
            var dependencyProperty = $requiredIfConditionControl.data('val-requiredif-dependentproperty');
            var id = '#' + prefix + dependencyProperty;
            var $element = $(id);

            if ($element.is("input")) {
                $element.on('blur', function () {
                    $requiredIfConditionControl.valid();
                });
            }
            if ($element.is("select")) {
                $element.on('change', function () {
                    $requiredIfConditionControl.valid();
                });
            }
            var $document = $(document);
            $document.ready(function () {
                $requiredIfConditionControl.valid();
            });               
        }        
    });
})();
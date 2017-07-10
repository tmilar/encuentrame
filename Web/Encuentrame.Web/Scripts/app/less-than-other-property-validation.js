(function () {
    $.validator.addMethod('lessthanotherproperty',
        function (value, element, parameters) {
            if (typeof (value) == 'undefined') {
                return true;
            }

            var otherProperty = parameters['otherproperty'];

            var $me=$(element);
            var $control = $('#' + otherProperty);

            if ($control.length === 0) {
                $control = $('#' + otherProperty + '_Id');
                if ($control.length === 0) {
                    var idStr = $me.attr("id");
                    var prefix = idStr.substring(0, idStr.indexOf('__'));
                    console.log(prefix);
                    $control = $('#' + prefix + "__" + otherProperty);
                    if ($control.length === 0) {
                        $control = $('#' + prefix + "__" + otherProperty + '_Id');
                    }
                }
            }

            var otherValue = $control.val();
            if ($control.hasClass("datetime-control") || $control.hasClass("date-control")) {
                return value === "" || value === undefined || value === null || value > otherValue;
            } else {
               
                return parseInt(value) <= parseInt(otherValue);
            }            
        }
    );
    $.validator.unobtrusive.adapters.add('lessthanotherproperty', ['otherproperty'], function (options) {
        var parameters = {
            otherproperty: options.params['otherproperty']
        };

        options.rules['lessthanotherproperty'] = parameters;
       
        options.messages['lessthanotherproperty'] = options.message;
    });

    
})();
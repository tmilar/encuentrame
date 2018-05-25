(function () {
    $.validator.addMethod('cuit',
        function (value, element, parameters) {
            if (typeof (value) == 'undefined') {
                return true;
            }

            var cuit = value.toString().replace(/[-_]/g, "");
            if (cuit === '') {
                return true;
            }

            if (cuit.length !== 11) {
                return false;
            }

            var multipliers = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
            var total = 0;
            for (var i = 0; i < multipliers.length; i++) {
                total += parseInt(cuit[i]) * multipliers[i];
            }
            var mod = total % 11;
            var digit = mod === 0 ? 0 : mod === 1 ? 9 : 11 - mod;

            return digit === parseInt(cuit[10]);

        }
    );
    $.validator.unobtrusive.adapters.add('cuit', function (options) {
        options.rules['cuit'] = {};
        options.messages['cuit'] = options.message;
    });

    
})();
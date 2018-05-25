(function () {
    $(".alert.alert-success.alert-dismissible.alert-auto-dismissible").each(function (idx, obj) {
        setTimeout(function() {
            var $obj = $(obj);
            $obj.animate({
                opacity: '0',
                height: '0'
            }, "slow", function () {
                $obj.remove();
            });
            
        }, 3000);
    });
    moment.locale('es');
    
})();
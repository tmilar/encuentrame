(function () {
    $(document).ready(function () {
        $(".disabled-group-control").makeDisabledGroup({
            sourceSelector: "input[type=checkbox]",
            sourceValueToDisabled: true,
            targetSelector: ".reference2"
        });
    });
})();
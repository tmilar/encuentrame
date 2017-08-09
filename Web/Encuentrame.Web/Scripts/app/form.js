// expresion de funcion nombrada
var configureThisControls = function configureThisControls($dom) {

    $dom.find('.mask-control').each(function (idx, obj) {
        var $obj = $(obj);
        var mask = $obj.attr("data-mask-pattern");
        //var reverse = $obj.attr("data-reverse") === 'True' ? true : false;
        var placeholder = $obj.attr("data-placeholder");
        $obj.mask(mask, {
            reverse: false,
            placeholder: placeholder,
            translation: {
                '0': {
                    pattern: /[0-9]/, optional: true
                },
                '~': {
                    pattern: /[+-]/, optional: true

                }
            }
        });
    });

    $dom.find('.date-control').datepicker({
        format: "dd/mm/yyyy",
        startDate: "01/01/1920",
        language: "es",
        autoclose: true,
        todayHighlight: true
    });


    $dom.find('.datetime-control').each(function (idx, ctrl) {
        var $this = $(ctrl);
        var options = {
            format: "DD/MM/YYYY HH:mm",
            minDate: "01/01/1920",

            locale: 'es',
            keepOpen: true,
        }

        if ($this.data("current-is-max-datetime")) {
            $.extend(options, { maxDate: moment() });
        }
        $this.datetimepicker(options);
    });
    $dom.find('.reference-enum').select2({
        language: "es",
        minimumResultsForSearch: -1
    });

    $dom.find('.reference-single, .reference-any, .reference-related').select2({
        language: "es"
    });



    $dom.find('.reference-any').on("change", function () {
        var $this = $(this);
        var idPrefix = $this.attr("id").replace("referenceAny", "");
        var $typeTarget = $("#" + idPrefix + "Type");
        var $idTarget = $("#" + idPrefix + "Id");
        if ($this.val() === "") {
            $typeTarget.val("");
            $idTarget.val("");
        } else {
            var values = $this.val().split('#');
            $typeTarget.val(values[0]);
            $idTarget.val(values[1]);
        }
    });



    $dom.find('.reference-related').each(function (idx, select) {
        var $select = $(select);
        var relatedControls = {};

        var relatedNames = $select.data("related-to").split("|");

        if (relatedNames.length === 1) {
            relatedControls['relatedId'] = $select.findRelatedControl();
        } else {
            $.each(relatedNames,
                function(index1, name) {
                    relatedControls[name.pascalCase()] = $select.findRelatedControl(name);
                });
        }

      

        var selectedId = $select.data('selected-id');
        $select.removeAttr('data-selected-id');
        if (selectedId == undefined) {
            selectedId = '';
        }
        var url = $select.data('source-url');

        var setSelect = function (dataSource) {
            if ($select.prop("multiple")) {
                $select.find('option').remove();
            } else {
                $select.find('option').slice(1).remove();
            }

            $select.select2({
                language: "es",
                data: dataSource
            });

            if ($select.prop("multiple")) {
                if ($.isNumeric(selectedId)) {
                    $select.val(selectedId).trigger('change');
                } else {
                    var selectedIds = null;
                    if (selectedId !== null && selectedId !== undefined) {
                        selectedIds = selectedId.split('|');
                    }
                    $select.val(selectedIds).trigger('change');
                }

            } else {
                $select.val(selectedId).trigger('change');
            }
        }



        var getData = function () {
            var parameters = {
                selectedId: selectedId ? selectedId : -1
            };


            var isIncomplete = false;
            $.each(relatedControls,
                function (index, $obj) {
                    var relatedId = $obj.val();
                    if (relatedId === '' || relatedId === undefined || relatedId === null) {
                        isIncomplete = true;
                    }
                    parameters[index] = relatedId;
                });
            if (isIncomplete) {
                setSelect([]);
                return;
            }

            $.get(url, parameters)
                .done(function (data) {
                    if (data.Status) {
                        setSelect(data.Info);
                        selectedId = null;
                    }
                }).fail(function () {
                    setSelect([]);
                });
        }

        $.each(relatedControls,
            function (index, $obj) {
                $obj.on("change", function () {
                    getData();
                });
            });

       

        var isComplete = true;
        $.each(relatedControls,
            function (index, $obj) {
                if ($obj.val() === '' || $obj.val() === undefined || $obj.val() === null) {
                    isComplete = false;

                }
            });
        if (isComplete) {
            getData();
        }



    });

    $dom.find("textarea").each(function (idx, textarea) {
        var $textarea = $(textarea);
        var maxlength = $textarea.attr("maxlength");
        if (maxlength > 0) {
            $textarea.textareaCount({
                'maxCharacterSize': maxlength,
                'originalStyle': 'text-muted',
                'warningStyle': 'text-danger',
                'warningNumber': 20,
                'displayFormat': '#input/#max'
            });
        }
    });

    $dom.find(".token-field").each(function (idx, input) {
        var $input = $(input);
        var type = "text";

        if ($input.data("token-type") === "Numbers") {
            type = "number";
        }
        $input.tokenfield({
            delimiter: [',', " ", ";"],
            inputType: type
        });
    });

    $dom.find(".image-control").each(function (idx, obj) {
        var $this = $(obj);
        var $imageControl = $this.find("img");
        var $fileControl = $this.find("input[type=file]");
        var $hiddenControl = $this.find("input[type=hidden]");
        var $buttonControl = $this.find("a.btn-upload");

        $buttonControl.click(function () {
            $fileControl.click();
        });

        $fileControl.change(function () {
            var data = new FormData();
            var files = $fileControl.get(0).files;
            if (files.length > 0) {
                data.append("image", files[0]);
            }
            console.log("URL: " + $this.data("image-url"));
            $.ajax({
                url: $this.data("image-url"),
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (response) {
                    if (response.isValid) {
                        var imageSource = "data:image/jpeg;base64," + response.image;
                        $imageControl.attr("src", imageSource);
                        $hiddenControl.val(imageSource);
                    }
                },
                error: function (er) {
                    console.log(er);
                }
            });


        });
    });

    $dom.find('[data-toggle="tooltip"]').tooltip();
};

(function () {
    $(document).ready(function () {
        configureThisControls($(document));
    });
})();

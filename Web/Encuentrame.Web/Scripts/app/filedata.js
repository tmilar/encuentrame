var setupFileSection = function ($dom, $fileSection) {
    var reader;    
    var $fileListContainer = $fileSection.find('.file-list');
    var $progressContainer = $fileSection.find('.progress');
    var $progress = $progressContainer.find('.progress-bar');
    var $fileControl = $fileSection.find('input[type=file]');
    var $buttonControl = $fileSection.find("a.btn-upload");
    var $itemsContainer = $fileSection.find(".file-item");
    var outerHtmlTemplate;
    var maxFileCountString = $fileSection.data('max-file-count');
    var maxFileCount = 0;
    var validateFileCount = false;
    var maxFileSizeString = $fileSection.data('max-file-size');
    var maxFileSize = 0;
    var validateFileSize = false;
    var fileExtensionsString = $fileSection.data('allowed-extensions');    
    var fileEmptyError = $fileSection.data('file-empty-error');
    var extensionNotAllowedError = $fileSection.data('extension-not-allowed-error');
    var maxFileSizeError = $fileSection.data('max-file-size-error');
    var $errorsDiv = $fileSection.find('.for-error');
    var errorTemplateId = "errorMessageTemplate";
    extensionNotAllowedError = extensionNotAllowedError.replace("{1}", fileExtensionsString);
    maxFileSizeError = maxFileSizeError.replace("{1}", maxFileSizeString);
    var errorTemplate = $("#" + errorTemplateId).html();

    function updateProgessBar(percentage) {
        var newPercentage = percentage + '%';
        $progress.css("width", newPercentage);
        $progress.html(newPercentage);
    }

    function resetFileUpload() {
        // Reset progress indicator on new file selection.
        updateProgessBar(0);
    }

    function hideFileUpload() {
        resetFileUpload();
        $progressContainer.hide();
    }

    function showFileUpload() {
        resetFileUpload();
        $progressContainer.show();
    }

    function validateCanAddMoreFiles() {
        if (validateFileCount) {
            var itemsCount = $fileListContainer.find(".file-item").length;
            if (itemsCount >= maxFileCount)
                $buttonControl.hide();
            else
                $buttonControl.show();
        }
    }

    hideFileUpload();
    
    var fileExtensions;
    if (fileExtensionsString !== "") {

        fileExtensions = fileExtensionsString.split(',');
    }

    if (maxFileCountString !== undefined && maxFileCountString !== null) {
        maxFileCount = parseInt(maxFileCountString);
        if (maxFileCount !== 0)
            validateFileCount = true;
    }
    validateCanAddMoreFiles();

    if (maxFileSizeString !== undefined && maxFileSizeString !== null) {
        maxFileSize = parseInt(maxFileSizeString);
        if (maxFileSize !== 0)
            validateFileSize = true;
    }

    function showError(errorMessage) {
        $errorsDiv.empty();

        var error = _.template(errorTemplate)({ errorMessage: errorMessage });
        $errorsDiv.append(error);
        $errorsDiv.show();
    }

    function isFileSizeValid(currentFileSize, fileName) {
        var errorMessage = "";
        if (currentFileSize === 0) {
            errorMessage = fileEmptyError.replace("{0}", fileName);
            showError(errorMessage);
            return false;
        }

        if (validateFileSize && currentFileSize > maxFileSize) {
            errorMessage = maxFileSizeError.replace("{0}", fileName);
            showError(errorMessage);
            return false;
        }
        return true;
    }

    $buttonControl.click(function () {
        $fileControl.click();
    });

    function updateFileRowIds() {
        $.updateNamesAndIds($fileSection.find('.file-list'), { rowSelector: '.file-item' });
    }

    function findTemplate() {
        outerHtmlTemplate = $itemsContainer[0].outerHTML;
        
        var idHidden = $($itemsContainer[0]).find('.file-id');
        if(idHidden.val() == -1)
            $itemsContainer[0].remove();
        updateFileRowIds();
    }

    findTemplate();

    //function abortRead() {
    //    reader.abort();
    //}

    function errorHandler(evt) {
        switch (evt.target.error.code) {
            case evt.target.error.NOT_FOUND_ERR:
                alert('File Not Found!');
                break;
            case evt.target.error.NOT_READABLE_ERR:
                alert('File is not readable');
                break;
            case evt.target.error.ABORT_ERR:
                break; // noop
            default:
                alert('An error occurred reading this file.');
        };
    }

    function updateProgress(evt) {
        // evt is an ProgressEvent.
        if (evt.lengthComputable) {
            var percentLoaded = Math.round((evt.loaded / evt.total) * 100);
            // Increase the progress bar length.
            if (percentLoaded < 100) {
                updateProgessBar(percentLoaded);
            }
        }
    }    

    function removeFile() {
        var $parentRow = $(this).parents(".file-item:first");
        $parentRow.remove();
        updateFileRowIds();
        validateCanAddMoreFiles();
    }

    function handleFileSelect(evt) {        
        $errorsDiv.hide();
        if (evt.currentTarget.files.length > 0) {
            var newFile = evt.currentTarget.files[0].name;
            var fileSize = evt.currentTarget.files[0].size;
            var lasIndexOfDot = newFile.lastIndexOf(".");
            var extension = "";

            if (lasIndexOfDot !== -1)
                extension = newFile.substr(lasIndexOfDot + 1).toLowerCase(); // Contains 24 //

            if (fileExtensionsString === "" || (fileExtensions !== undefined && fileExtensions !== null && fileExtensions.length > 0 && $.inArray(extension, fileExtensions) !== -1)) {
                if (isFileSizeValid(fileSize, newFile)) {
                    showFileUpload();

                    reader = new FileReader();
                    reader.onerror = errorHandler;
                    reader.onprogress = updateProgress;
                    reader.onabort = function(e) {
                        alert('File read cancelled');
                    };

                    reader.onload = function(e) {
                        // Ensure that the progress bar displays 100% at the end and then hides.
                        updateProgessBar(100);
                        setTimeout(hideFileUpload, 1000);

                        var $items = $fileListContainer.find(".file-item");
                        var $newFileItem = $(outerHtmlTemplate);

                        var $hiddenValue = $newFileItem.find('.file-data');
                        var $hiddenId = $newFileItem.find('.file-id');
                        $hiddenId.val(0);

                        var $fileNameLabel = $newFileItem.find(".file-name");
                        $fileNameLabel.text(newFile);
                        $fileNameLabel.show();
                        var $fileLink = $newFileItem.find(".file-link");
                        $fileLink.remove();                        
                        
                        var $fileNameHidden = $newFileItem.find(".file-name-hidden");
                        $fileNameHidden.val(newFile);

                        var fileContent = e.target.result;
                        var mimeTypeString = fileContent.substr(0, fileContent.indexOf(',') + 1);
                        fileContent = fileContent.replace(mimeTypeString, '');
                        $hiddenValue.val(fileContent);
                        var $fileType = $newFileItem.find(".file-type");
                        $fileType.val(mimeTypeString);

                        var $fileSize = $newFileItem.find(".file-size");
                        $fileSize.val(fileSize);

                        var $fileExtension = $newFileItem.find(".file-extension");
                        $fileExtension.val(extension);

                        var $remove = $newFileItem.find('.remove-row');
                        $remove.click(removeFile);

                        if ($items.length !== 0) {
                            $newFileItem.insertAfter($($items[$items.length - 1]));
                        } else {
                            $fileListContainer.append($newFileItem);
                        }

                        //$newFileItem.show();
                        updateFileRowIds();
                        validateCanAddMoreFiles();
                    }

                    // Read in the image file as a binary string.
                    reader.readAsDataURL(evt.target.files[0]);
                }
            } else {
                var errorMessage = extensionNotAllowedError.replace("{0}", extension);
                showError(errorMessage);
            }
        }
    }

    var $remove = $itemsContainer.find('.remove-row');
    $remove.click(removeFile);
    $fileControl[0].addEventListener('change', handleFileSelect, false);
};

var configureFileSections = function ($dom) {    
    $dom.find('.file-upload-section').each(function (idx, obj) {
        setupFileSection($dom, $(this));
    });
};

(function () {
    var $document = $(document);
    $document.ready(function () {
        configureFileSections($document);
    });
})();
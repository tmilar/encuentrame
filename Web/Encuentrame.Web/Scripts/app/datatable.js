function isArray(obj) {
    return !!obj && Array === obj.constructor;
}

function fnFormatDetails(tableId, $detailsTable) {
    var detailsTableHtml = $detailsTable[0].outerHTML;
    var $detailsTableHtml = $(detailsTableHtml);
    $detailsTableHtml.attr("id", tableId);
    $detailsTableHtml.show();
    return $detailsTableHtml[0].outerHTML;
}

function getIndex(jqueryObj, hasDetails, hasSelection) {
    var index = parseInt(jqueryObj.data("index"));

    if (hasDetails || hasSelection)
        index = index + 1;
    return index;
}

function processTable(table, jsonData, addChildTable, addFilters, isChildTable) {
    var $table = table;
    var dataUrl = $table.data('url');
    var $ths = $table.find("th");
    var columnDefs = [];
    var columnsToTotalize = [];
    var columns = [];
    var columnNames = [];
    var tableId = $table[0].id;
    var detailsName = tableId + "_Details";
    var $detailsTable = $('table[id^=' + detailsName + ']');
    var hasDetails = false;
    var hasSelection = false;
    var sortColumnIndex = 0;
    var parentId = $table.data("parent-id");

    var allowSelection = $table.data("allow-selection");
    if (allowSelection) {
        hasSelection = true;
        hasDetails = false;
        sortColumnIndex = sortColumnIndex + 1;
        var newColumn =
        {
            "orderable": false,
            "className": 'select-checkbox',
            "targets": 0,
            "data": null,
            "defaultContent": ''
        };

        columns.push(newColumn);
        columnNames.push('check');
    }

    if ($detailsTable.length > 0 && jsonData == null) {
        hasDetails = true;
        sortColumnIndex = sortColumnIndex + 1;
        $detailsTable.hide();
        var newColumn =
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": ''
        };
        columns.push(newColumn);
        columnNames.push('details');
    }
    $table.data("hasDetails", hasDetails);
    $table.data("allow-selection", allowSelection);
    $ths.each(function (index, th) {
        var $th = $(th);
        var dataName = $th.data("name");
        var dataTemplate = $th.data("th-template");
        var type = $th.data("type");
        var isReference = $th.data("is-reference");
        var columnIndex = getIndex($th, hasDetails, hasSelection);
        var totalize = $th.data("totalize");

        if (totalize)
            columnsToTotalize.push(columnIndex);

        if (dataTemplate != null) {
            var columnDef = {
                "targets": columnIndex,
                //"data": null,
                "mData": function (source, type, val) {
                    var template = $("#" + dataTemplate).html();
                    if (dataName == null) {
                        return _.template(template)({ row: source });
                    } else {
                        var name = dataName;
                        if (type === 'set') {
                            source[name] = val;
                            source[name + "_filter "] = source[name];
                            return;
                        } else if (type === 'display') {
                            return _.template(template)({ row: source });
                        } else if (type === 'filter') {
                            return source[name];
                        }
                        // 'sort', 'type' and undefined all just use the integer
                        return source[name];
                    }
                }
            };
            columnDefs.push(columnDef);
        }
        else if (type != null && type === "DateTime") {
            var columnDef = {
                "targets": columnIndex,
                "data": null,
                "render": function (data, type, row) {
                    if (data) {
                        var dateWrapper = moment(data);
                        // Order and type get a number value from Moment, everything else
                        // sees the rendered value
                        return dateWrapper.format(type === 'sort' || type === 'type' ? 'x' : 'DD/MM/YYYY HH:mm');
                    }
                    return "";
                }
            };
            columnDefs.push(columnDef);
        }
        else if (isReference) {
            var idPath = $th.data("id-path");
            var namePath = $th.data("name-path");
            var columnDef = {
                "targets": columnIndex,
                "data": null,
                "render": function (data, type, row) {
                    if (isArray(data)) {
                        var returnValue = "";
                        if (data.length > 0) {
                            returnValue = data[0][namePath];
                            for (var i = 1, l = data.length; i < l; i++) {
                                returnValue = returnValue + ", " + data[i][namePath];
                            }
                        }
                        return returnValue;
                    } else {
                        if (data != null) {
                            return data[namePath];
                        }
                        return "";
                    }


                }
            };
            columnDefs.push(columnDef);
        }

        if (dataName != null) {
            var column = {}
            if (dataTemplate == null) {
                column.data = dataName;
            };

            var thIsVisible = $th.data("visible");
            if (thIsVisible != null) {
                column.visible = thIsVisible;
            }

            var thIsSortable = $th.data("sortable");
            if (thIsSortable != null) {
                column.sortable = thIsSortable;
            }

            columns.push(column);
            columnNames.push(dataName);
        }
    });

    var serverSide = $table.data("server-side");
    var allowSearch = $table.data("allow-search");


    var dataTableConfig = {
        "drawCallback": function (settings) {
            $table.find('[data-toggle="tooltip"]').tooltip({
                trigger: 'hover'
            });
            $table.find('[data-toggle2="tooltip"]').tooltip({
                trigger: 'hover'
            });
        },
        cache: true,
        //sort: false,     
        "searching": allowSearch,
        "processing": serverSide,
        "serverSide": serverSide,
        "deferLoading": 0,
        "deferRender": true,
        //"columns": columns,
        dom: 'lrtip', //https://datatables.net/reference/option/dom
        //stateSave: true,
        "language": {
            "url": "Scripts/app/datatable-spanish.js"
        }
        //"ajax": ,
        //"columnDefs": columnDefs        
    };

    if ($table.data('sort-column') !== undefined) {
        var sortIndex = $table.data("sort-column");
        sortColumnIndex = sortColumnIndex + sortIndex;
        var defaultOrder = $table.data("default-order");
        dataTableConfig["order"] = [[sortColumnIndex, defaultOrder]];
    }

    var lengthsToShow = [[5, 10, 20, -1], [5, 10, 20, "Todos"]];
    var pageLength = 10;
    if ($table.data('hide-show-all') !== undefined || $table.data('lengths-to-show') !== undefined) {
        var options = [];
        var optionsIndex = [];

        if ($table.data('lengths-to-show') !== undefined) {
            var lengths = $table.data('lengths-to-show').split(',');
            $.each(lengths, function (index, length) {
                var lengthNumeric = parseInt(length);
                options.push(lengthNumeric);
                optionsIndex.push(lengthNumeric);
            });
            var defaultOption = options[0];
            if ($table.data('default-length') !== undefined) {
                defaultOption = $table.data('default-length');
            }

            pageLength = defaultOption;
        } else {
            options = [5, 10, 20];
            optionsIndex = [5, 10, 20];
            pageLength = 10;
        }

        if ($table.data('hide-show-all') !== undefined) {
            var hideShowAll = $table.data('hide-show-all');
            if (!hideShowAll) {
                options.push(-1);
                optionsIndex.push("Todos");
            }
        }

        lengthsToShow = [options, optionsIndex];
    }

    if (columnsToTotalize.length > 0) {
        dataTableConfig["footerCallback"] = function (tfoot, data, start, end, display) {
            var columnNumbers = columnsToTotalize;
            var tableReference = this;
            $.each(columnNumbers, function (index, th) {
                var api = tableReference.api();
                var columnNumber = this;
                var total = 0;
                // Total over all pages
                if (api.column(columnNumber, { search: 'applied' }).data().length) {
                    total = api
                        .column(columnNumber, { search: 'applied' })
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        });
                }
                else { total = 0 };

                $(api.column(columnNumber).footer()).html(
                    total
                );
            });
        };
    }

    var ajaxConfig = {
        "url": dataUrl,
        async: false,
        type: "POST",
        //datatype: "json",
        //contentType: "application/json; charset=utf-8",
        "data": function (d) {
            var data = {

            }
            var searchColumns = [];
            //$(d.columns).each(function (index, column) {
            //    if (column.search.value !== "") {
            //        var searchData = {
            //            Column: column.data,
            //            Value: column.search.value,
            //            Regex: column.search.regex,
            //            Index: index
            //        }
            //        searchColumns.push(searchData);
            //    }
            //});
            addFilters(searchColumns, isChildTable);
            var dataTableModel = {
                SearchData: searchColumns,
                Draw: d.draw,
                Length: d.length,
                Start: d.start,
                IsServerSide: serverSide
            };
            if (d.order !== undefined && d.order.length > 0) {
                var orderInfo = d.order[0];
                var column = orderInfo.column;
                if (column === 0 && (hasDetails || hasSelection))
                    column = column + 1;

                dataTableModel.SortColumn = columnNames[column];
                dataTableModel.SortOrder = orderInfo.dir;
            }
            if (parentId !== undefined && parentId !== "")
                dataTableModel.ParentId = parentId;
            data = dataTableModel;
            //return JSON.stringify(data);
            return dataTableModel;
        }
    }

    if (jsonData == null) {
        dataTableConfig["ajax"] = ajaxConfig;
        dataTableConfig["lengthMenu"] = lengthsToShow;
        dataTableConfig["pageLength"] = pageLength;
        dataTableConfig["lengthChange"] = true;
        dataTableConfig["paging"] = true;
    } else {
        dataTableConfig["paging"] = false;
        dataTableConfig["data"] = jsonData;
        dataTableConfig["dom"] = 'lrt';
        dataTableConfig["lengthChange"] = false;
    }

    dataTableConfig["columns"] = columns;

    if (allowSelection) {
        dataTableConfig["select"] =
        {
            style: 'multi',
            selector: 'td:first-child'
        };
    }

    if (columnDefs.length > 0) {
        dataTableConfig["columnDefs"] = columnDefs;
    }

    var myJqueryTable = $table.DataTable(dataTableConfig);
    dataTableConfig[""] = myJqueryTable;
    //$table.DataTable(dataTableConfig);

    if (hasDetails) {
        var detailsProperty = "";
        var parentIdProperty = "";
        var detailsTableId = $detailsTable[0].id;
        $detailsTable = $("#" + detailsTableId);

        var detailsIsServerSide = $detailsTable.data("server-side");

        if (detailsIsServerSide) {
            parentIdProperty = $table.data("parent-property");
        }
        else {
            var values = detailsTableId.split('_');
            detailsProperty = values[values.length - 1];
        }

        $('#' + tableId + ' tbody').on('click', 'td.details-control', function () {
            var tr = $(this).parents('tr:first');
            var row = myJqueryTable.row(tr);
            var rowDetailsTableId = detailsTableId + "_" + row.index();

            var $relatedChildTable;

            if (row.child.isShown()) {
                // This row is already open - close it
                $relatedChildTable = $("#" + rowDetailsTableId);
                var childDataTable = $relatedChildTable.DataTable({ "retrieve": true });
                childDataTable.destroy(true);
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                tr.addClass('shown');
                // Open this row
                row.child(fnFormatDetails(rowDetailsTableId, $detailsTable)).show();

                //var data = {
                //    "data": items,
                //    recordsTotal: items.length,
                //    recordsFiltered: items.length
                //};

                $relatedChildTable = $("#" + rowDetailsTableId);
                var jqueryChildTable;
                if (detailsIsServerSide) {
                    var parentId = row.data()[parentIdProperty];
                    $relatedChildTable.data("parent-id", parentId);
                    jqueryChildTable = processTable($relatedChildTable, null, addChildTable, addFilters, true);

                } else {
                    var items = row.data()[detailsProperty];
                    jqueryChildTable = processTable($relatedChildTable, items, addChildTable, addFilters, true);
                }

                var childTabledata =
                {
                    tableId: rowDetailsTableId,
                    $relatedChildTable: $relatedChildTable,
                    jqueryChildTable: jqueryChildTable,
                    hasDetails: false,
                    allowSelection: false,
                    tr: tr
                }

                addChildTable(childTabledata);

                jqueryChildTable.draw();
            }
        });

        //$detailsContainer.hide();
        if (jsonData != null) {
            myJqueryTable.fnClearTable();
            myJqueryTable.fnAddData(jsonData);
        }
    }

    if (hasSelection) {
        if (jsonData != null) {
            myJqueryTable.fnClearTable();
            myJqueryTable.fnAddData(jsonData);
        }

        myJqueryTable.on("click", "th.select-checkbox", function () {
            if ($("th.select-checkbox").hasClass("selected")) {
                myJqueryTable.rows().deselect();
                $("th.select-checkbox").removeClass("selected");
            } else {
                myJqueryTable.rows().select();
                $("th.select-checkbox").addClass("selected");
            }
        }).on("select deselect", function () {
            if (myJqueryTable.rows({
                selected: true
            }).count() !== myJqueryTable.rows().count()) {
                $("th.select-checkbox").removeClass("selected");
            } else {
                $("th.select-checkbox").addClass("selected");
            }
        });

    }

    return myJqueryTable;
}

var setupTable = function ($dom, table, jsonData) {
    var $tableDomObj = table;

    var rootTableData =
    {
        $tableDomObj: $tableDomObj,
        tableId: $tableDomObj[0].id,
        columns: [],
        serverSide: $tableDomObj.data("server-side"),
        hasDetails: false,
        deferredDraw: false,
        childTables: {},
        inputFilters: [],
        intFilters: [],
        booleanFilters: [],
        rangeFilters: [],
        dateRangeFilters: [],
        enumFilters: [],
        selectFilters: [],
        childrenFilters: [],
        childrenNotRegularFilters: [],
        currentFilters: [],
        currentChildrenFilters: [],
        currentNotRegularFilters: [],
        currentNotRegularChildrenFilters: [],
        allRootFilters: [],
        allChildrenFilters: []
    }

    function addChildTable(childTableInfo) {

        var childTableData = {
            tableId: childTableInfo.tableId,
            table: childTableInfo.jqueryChildTable,
            hasDetails: childTableInfo.hasDetails,
            allowSelect: childTableInfo.allowSelect
        }
        rootTableData.childTables[childTableInfo.tableId] = childTableData;

        $(rootTableData.currentChildrenFilters).each(function (idx, obj) {
            var currentFilter = this;
            currentFilter.filterTable(childTableInfo.jqueryChildTable, childTableInfo.hasDetails, childTableInfo.allowSelect);
        });

        childTableInfo.$relatedChildTable.on('destroy.dt', function (e, settings) {
            delete rootTableData.childTables[childTableInfo.tableId];
        });
    }

    function addFilters(searchColumns, isChildTable) {

        if (isChildTable) {
            $.each(rootTableData.currentNotRegularChildrenFilters, function (index, item) {
                var filter = item;
                var searchData = {
                    Column: filter.columnName,
                    Value: filter.getValue(),
                    Regex: true,
                    Index: filter.columnIndex,
                    FilterType: filter.type
                }
                searchColumns.push(searchData);
            });
            $.each(rootTableData.currentChildrenFilters, function (index, item) {
                var filter = item;
                var searchData = {
                    Column: filter.columnName,
                    Value: filter.getValue(),
                    Regex: true,
                    Index: filter.columnIndex,
                    FilterType: filter.type
                }
                searchColumns.push(searchData);
            });
        } else {
            $.each(rootTableData.currentNotRegularFilters, function (index, item) {
                var filter = item;
                var searchData = {
                    Column: filter.columnName,
                    Value: filter.getValue(),
                    Regex: true,
                    Index: filter.columnIndex,
                    FilterType: filter.type
                }
                searchColumns.push(searchData);
            });
            $.each(rootTableData.currentFilters, function (index, item) {
                var filter = item;
                var searchData = {
                    Column: filter.columnName,
                    Value: filter.getValue(),
                    Regex: true,
                    Index: filter.columnIndex,
                    FilterType: filter.type
                }
                searchColumns.push(searchData);
            });
        }
    }

    rootTableData.jqueryTable = processTable($tableDomObj, jsonData, addChildTable, addFilters, false);
    rootTableData.hasDetails = $tableDomObj.data("hasDetails");

    function drawChildren() {
        if (!rootTableData.deferredDraw) {
            $.each(rootTableData.childTables, function (key, value) {
                var tableData = value;
                tableData.table.draw();
            });
        } else {
            rootTableData.filterChildren = true;
        }
    }

    function drawParent() {
        if (!rootTableData.deferredDraw)
            rootTableData.jqueryTable.draw();
        else {
            rootTableData.filterParent = true;
        }
    }

    $dom.find('.select-reference-filter').each(function (idx, obj) {
        var $selectFilter = $(this);

        var $filtersContainer = $selectFilter.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");

        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $selectFilter.parents(".table-filter-container:first").data("for-child-table");

            var columnIndex = parseInt($selectFilter.data("index"));
            var idPath = $selectFilter.data("id-path");
            var propertyPath = $selectFilter.data("name");
            var isMultiple = $selectFilter.data("is-multiple");

            var url = $selectFilter.data("url");
            //Set Default value.
            var defaultData = $selectFilter.data('default-value');
            var showGroups = $selectFilter.data('show-groups');

            $.ajax({
                url: url,
                type: "GET"
            }).done(function (result) {
                var currentGroup = "";
                if (result.Status) {
                    $.each(result.Info, function (index, item) {

                        if (showGroups) {
                            if (currentGroup !== item.group) {
                                if (currentGroup !== "") {
                                    $selectFilter.append('</optgroup>');
                                }

                                $selectFilter.append('<optgroup label="' + item.group + '">');
                                currentGroup = item.group;
                            }
                        }

                        $selectFilter.append($("<option></option>").text(item.text).val(item.id));

                    });

                    if (showGroups) {
                        $selectFilter.append('</optgroup>');
                    }

                    if (defaultData != null) {
                        if ($.isNumeric(defaultData)) {
                            $selectFilter.val(defaultData);
                        } else {
                            var defaultDataItems = defaultData.split('|');
                            $selectFilter.val(defaultDataItems);
                        }
                    }
                }

                var searchValue = defaultData;
                var selectFilterObj = {
                    type: "SelectFilter",
                    $selectFilter: $selectFilter,
                    forChildTable: forChildTable,
                    searchValue: searchValue,
                    defaultData: defaultData,
                    columnIndex: columnIndex,
                    columnName: $selectFilter.data("name"),
                    idPath: idPath,
                    propertyPath: propertyPath,
                    isMultiple: isMultiple,
                    getValue: function () {
                        if (this.isMultiple) {
                            var values = this.$selectFilter.select2("val");
                            if (typeof values !== 'undefined' && values !== null && values.length > 0)
                                return values.join('|');
                        }
                        else
                            return this.searchValue;
                        return "";
                    },
                    clean: function () {
                        this.$selectFilter.val(null).trigger('change.select2');
                    },
                    IsRowMatch: function (tableToFilter, hasDetails, allowSelect, searchData, rowData) {
                        var currentInputFilterObject = this;
                        var returnValue = true;

                        var property = rowData[currentInputFilterObject.propertyPath];

                        if (!currentInputFilterObject.searchValue || !currentInputFilterObject.searchValue.trim()) {
                            // is empty or whitespace
                            returnValue = returnValue && true;
                        } else {

                            var selectedValue = parseInt(currentInputFilterObject.searchValue);
                            if (isNaN(selectedValue)) {
                                returnValue = returnValue && true;
                            } else {
                                var currentId = property[idPath];

                                if (isNaN(selectedValue) ||
                                (currentId === selectedValue)) {
                                    returnValue = returnValue && true;
                                } else
                                    returnValue = returnValue && false;
                            }
                        }

                        return returnValue;
                    }
                }

                if (isMultiple) {
                    selectFilterObj.IsRowMatch = function (tableToFilter, hasDetails, allowSelect, searchData, rowData) {
                        var currentInputFilterObject = this;
                        var returnValue = true;

                        var property = rowData[currentInputFilterObject.propertyPath];

                        if (!currentInputFilterObject.searchValue || !currentInputFilterObject.searchValue.trim()) {
                            // is empty or whitespace
                            returnValue = returnValue && true;
                        } else {

                            var selectedValue = selectFilterObj.$selectFilter.select2("val");
                            var selectedValues = [];

                            if (isArray(selectedValue)) {
                                for (var i = 0; i < selectedValue.length; i++) {
                                    selectedValues.push(parseInt(selectedValue[i]));
                                }
                            } else {
                                selectedValues.push(parseInt(selectedValue));
                            }

                            var found = false;

                            if (property != null) {
                                if (isArray(property)) {
                                    for (var x = 0; x < selectedValues.length; x++) {
                                        var currentSelectedValue = selectedValues[x];
                                        for (var j = 0; j < property.length; j++) {
                                            var itemId = property[j][idPath];
                                            if (isNaN(currentSelectedValue) || (itemId === currentSelectedValue)) {
                                                found = true;
                                                break;
                                            }
                                        }
                                        if (found)
                                            break;
                                    }
                                } else {
                                    for (var x = 0; x < selectedValues.length; x++) {
                                        var currentSelectedValue = selectedValues[x];
                                        var itemId = property[idPath];
                                        if (isNaN(currentSelectedValue) || (itemId === currentSelectedValue)) {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            returnValue = returnValue && found;
                        }
                        return returnValue;
                    }
                }

                rootTableData.selectFilters.push(selectFilterObj);
                if (selectFilterObj.forChildTable) {
                    rootTableData.childrenNotRegularFilters.push(selectFilterObj);
                    rootTableData.allChildrenFilters.push(selectFilterObj);
                } else {
                    rootTableData.allRootFilters.push(selectFilterObj);
                }

                if (searchValue !== "" && searchValue !== undefined) {
                    if (!selectFilterObj.forChildTable) {
                        if ($.inArray(selectFilterObj, rootTableData.currentNotRegularFilters) === -1)
                            rootTableData.currentNotRegularFilters.push(selectFilterObj);
                        //rootTableData.jqueryTable.draw();
                    }
                }

                $selectFilter.on('change', function () {
                    var $filter = $(this);
                    var selectedValue = $filter.find("option:selected").val();;
                    var searchValue = selectedValue;

                    selectFilterObj.searchValue = searchValue;

                    if (selectFilterObj.forChildTable) {
                        if ($.inArray(selectFilterObj, rootTableData.currentNotRegularChildrenFilters) === -1)
                            rootTableData.currentNotRegularChildrenFilters.push(selectFilterObj);
                        drawChildren();
                    } else {
                        if ($.inArray(selectFilterObj, rootTableData.currentNotRegularFilters) === -1)
                            rootTableData.currentNotRegularFilters.push(selectFilterObj);
                        drawParent();
                    }
                });

            });
        }
    });

    $dom.find(".date-range-filter").each(function (idx, obj) {
        var $dateRangeFilterGroup = $(this);
        var $filtersContainer = $dateRangeFilterGroup.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");
        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $dateRangeFilterGroup.data("for-child-table");

            var $minDateRangeFilter = $dateRangeFilterGroup.find(".range-min");
            var $maxDateRangeFilter = $dateRangeFilterGroup.find(".range-max");

            var defaultMinDateRangeValue = $minDateRangeFilter.val();
            var defaultMaxDateRangeValue = $maxDateRangeFilter.val();

            var minDateRangeValue = defaultMinDateRangeValue;
            var maxDateRangeValue = defaultMaxDateRangeValue;

            var $dateRangeRow = $dateRangeFilterGroup.find(".date-range");
            var columnIndex = parseInt($dateRangeRow.data("index"));

            var dateRangeFilterObj = {
                type: "DateRangeFilter",
                $dateRangeFilterGroup: $dateRangeFilterGroup,
                $minDateRangeFilter: $minDateRangeFilter,
                minDateRangeValue: minDateRangeValue,
                defaultMinDateRangeValue: defaultMinDateRangeValue,
                $maxDateRangeFilter: $maxDateRangeFilter,
                defaultMaxDateRangeValue: defaultMaxDateRangeValue,
                maxDateRangeValue: maxDateRangeValue,
                forChildTable: forChildTable,
                columnIndex: columnIndex,
                columnName: $dateRangeRow.data("name"),
                getValue: function () {
                    var nadValue = "NaD";
                    var currentInputFilterObject = this;
                    var minString = currentInputFilterObject.minDateRangeValue;
                    var maxString = currentInputFilterObject.maxDateRangeValue;
                    if (minString === "" && maxString === "") {
                        return "";
                    } else if (minString === "") {
                        return nadValue + "|" + maxString;
                    } else if (maxString === "") {
                        return minString + "|" + nadValue;
                    }
                    return minString + "|" + maxString;
                },
                clean: function () {
                    this.$minDateRangeFilter.val('');
                    this.$maxDateRangeFilter.val('');
                },
                IsRowMatch: function (tableToFilter, hasDetails, allowSelect, searchData) {
                    var currentInputFilterObject = this;
                    var returnValue = true;

                    var minString = currentInputFilterObject.minDateRangeValue;
                    var maxString = currentInputFilterObject.maxDateRangeValue;
                    if (minString === "" && maxString === "") {
                        returnValue = returnValue && true;
                    } else {
                        var currentColumnIndex = currentInputFilterObject.columnIndex;
                        if (hasDetails || allowSelect)
                            currentColumnIndex = currentColumnIndex + 1;
                        var startDate = moment(minString, 'DD/MM/YYYY HH:mm');
                        var endDate = moment(maxString, 'DD/MM/YYYY HH:mm');
                        var date = moment(searchData[currentColumnIndex], 'DD/MM/YYYY HH:mm');
                        if ((minString === "" && date.isBefore(endDate)) ||
                            (date.isAfter(startDate) && maxString === "") ||
                            (date.isAfter(startDate) && date.isBefore(endDate)) ||
                            date.isSame(startDate) || date.isSame(endDate)) {
                            returnValue = returnValue && true;
                        } else
                            returnValue = returnValue && false;
                    }

                    return returnValue;
                }
            }

            if (dateRangeFilterObj.forChildTable) {
                rootTableData.childrenNotRegularFilters.push(dateRangeFilterObj);
                rootTableData.allChildrenFilters.push(dateRangeFilterObj);
            } else {
                rootTableData.allRootFilters.push(dateRangeFilterObj);
            }
            rootTableData.dateRangeFilters.push(dateRangeFilterObj);

            if (minDateRangeValue !== "" || minDateRangeValue !== "") {
                if (!dateRangeFilterObj.forChildTable) {
                    if ($.inArray(dateRangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                        rootTableData.currentNotRegularFilters.push(dateRangeFilterObj);
                    //rootTableData.jqueryTable.draw();
                }
            }

            $minDateRangeFilter.on('dp.change', function () {
                var $dateRangeFilter = $(this);

                var $rangeFilter = $(this).closest('.date-range-filter');
                var $minFilter = $(this);
                var $maxFilter = $rangeFilter.find('.range-max');

                var min = $minFilter.data("DateTimePicker").date();
                if (min != null) {
                    $maxFilter.data("DateTimePicker").minDate(min);
                }

                dateRangeFilterObj.minDateRangeValue = $dateRangeFilter.val();

                if (dateRangeFilterObj.forChildTable) {
                    if ($.inArray(dateRangeFilterObj, rootTableData.currentNotRegularChildrenFilters) === -1)
                        rootTableData.currentNotRegularChildrenFilters.push(dateRangeFilterObj);

                    drawChildren();

                } else {
                    if ($.inArray(dateRangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                        rootTableData.currentNotRegularFilters.push(dateRangeFilterObj);
                    drawParent();
                }
            });

            $maxDateRangeFilter.on('dp.change', function () {
                var $dateRangeFilter = $(this);

                var $rangeFilter = $(this).closest('.date-range-filter');
                var $maxFilter = $(this);
                var $minFilter = $rangeFilter.find('.range-min');

                var max = $maxFilter.data("DateTimePicker").date();
                if (max != null) {
                    $minFilter.data("DateTimePicker").maxDate(max);
                }


                dateRangeFilterObj.maxDateRangeValue = $dateRangeFilter.val();

                if (dateRangeFilterObj.forChildTable) {

                    if ($.inArray(dateRangeFilterObj, rootTableData.currentNotRegularChildrenFilters) === -1)
                        rootTableData.currentNotRegularChildrenFilters.push(dateRangeFilterObj);
                    drawChildren();
                } else {
                    if ($.inArray(dateRangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                        rootTableData.currentNotRegularFilters.push(dateRangeFilterObj);
                    drawParent();
                }
            });
        }
    });

    $dom.find('.select-enum-filter').each(function (idx, obj) {
        var $enumFilter = $(this);

        var $filtersContainer = $enumFilter.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");

        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $enumFilter.parents(".table-filter-container:first").data("for-child-table");

            var columnIndex = parseInt($enumFilter.data("index"));

            var defaultData = $enumFilter.data('default-value');

            if (defaultData != null) {
                if ($.isNumeric(defaultData)) {
                    $enumFilter.val(defaultData);
                } else {
                    defaultData = defaultData.split('|');
                    $enumFilter.val(defaultData);
                }
            }

            var searchValue = defaultData;

            var enumFilterObj = {
                type: "EnumFilter",
                $enumFilter: $enumFilter,
                forChildTable: forChildTable,
                searchValue: searchValue,
                defaultData: defaultData,
                columnIndex: columnIndex,
                columnName: $enumFilter.data("name"),
                getValue: function () {
                    return this.searchValue;
                },
                clean: function () {
                    this.$enumFilter.val(null).trigger('change.select2');
                },
                filterTable: function (tableToFilter, hasDetails, allowSelect, triggerDraw) {
                    triggerDraw = triggerDraw !== false;
                    var currentInputFilterObject = this;
                    var columnId = currentInputFilterObject.columnIndex;
                    if (hasDetails || allowSelect)
                        columnId = columnId + 1;

                    if (currentInputFilterObject.searchValue === null || currentInputFilterObject.searchValue === -1 || currentInputFilterObject.searchValue === undefined) {
                        tableToFilter.search("").column(columnId).search("");
                    } else {
                        tableToFilter.column(columnId).search(currentInputFilterObject.searchValue, true, false);
                    }

                    if (triggerDraw)
                        tableToFilter.draw();
                }
            }

            if (enumFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(enumFilterObj);
                rootTableData.allChildrenFilters.push(enumFilterObj);
            } else {
                rootTableData.allRootFilters.push(enumFilterObj);
            }
            rootTableData.enumFilters.push(enumFilterObj);

            if (searchValue !== "" && searchValue !== undefined) {
                if (!enumFilterObj.forChildTable) {
                    if ($.inArray(enumFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(enumFilterObj);

                    enumFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, false);
                } else {
                    if ($.inArray(enumFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(enumFilterObj);
                }
            }

            $enumFilter.on('change', function () {
                var $filter = $(this);
                var selectedValue = $filter.val();
                var searchValue = selectedValue;

                if (searchValue !== null) {
                    if (isArray(selectedValue)) {
                        searchValue = selectedValue.join("|");
                    } else {
                        searchValue = parseInt(selectedValue);
                    }
                }
                enumFilterObj.searchValue = searchValue;

                if (enumFilterObj.forChildTable) {
                    if ($.inArray(enumFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(enumFilterObj);

                    $.each(rootTableData.childTables, function (key, value) {
                        var tableData = value;
                        enumFilterObj.filterTable(tableData.table, tableData.hasDetails, tableData.allowSelect, !rootTableData.deferredDraw);
                    });
                    rootTableData.filterChildren = true;
                } else {
                    if ($.inArray(enumFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(enumFilterObj);
                    enumFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, !rootTableData.deferredDraw);
                    rootTableData.filterParent = true;
                }
            });
        }
    });

    $dom.find(".range-filter").each(function (idx, obj) {
        var $rangeFilterGroup = $(this);
        var $filtersContainer = $rangeFilterGroup.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");
        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $rangeFilterGroup.data("for-child-table");

            var $minRangeFilter = $rangeFilterGroup.find(".range-min");
            var $maxRangeFilter = $rangeFilterGroup.find(".range-max");

            var minString = $minRangeFilter.val();
            var maxString = $maxRangeFilter.val();

            var defaultMinRangeValue = parseFloat(minString, 10);
            var defaultMaxRangeValue = parseFloat(maxString, 10);

            var minRangeValue = defaultMinRangeValue;
            var maxRangeValue = defaultMaxRangeValue;

            var $rangeRow = $rangeFilterGroup.find(".range");
            var columnIndex = parseInt($rangeRow.data("index"));


            var rangeFilterObj = {
                type: "RangeFilter",
                $rangeFilterGroup: $rangeFilterGroup,
                $minRangeFilter: $minRangeFilter,
                minRangeValue: minRangeValue,
                defaultMinRangeValue: defaultMinRangeValue,
                $maxRangeFilter: $maxRangeFilter,
                defaultMaxRangeValue: defaultMaxRangeValue,
                maxRangeValue: maxRangeValue,
                forChildTable: forChildTable,
                columnIndex: columnIndex,
                columnName: $rangeRow.data("name"),
                getValue: function () {
                    var nanValue = "NaN";
                    var currentInputFilterObject = this;
                    var min = currentInputFilterObject.minRangeValue;
                    var max = currentInputFilterObject.maxRangeValue;
                    if (isNaN(min) && isNaN(max)) {
                        return "";
                    } else if (isNaN(min)) {
                        return nanValue + "|" + max;
                    } else if (isNaN(max)) {
                        return min + "|" + nanValue;
                    }
                    return min + "|" + max;
                },
                clean: function () {
                    this.$minRangeFilter.val('');
                    this.$maxRangeFilter.val('');
                },
                IsRowMatch: function (tableToFilter, hasDetails, allowSelect, searchData) {
                    var currentInputFilterObject = this;
                    var returnValue = true;

                    var min = currentInputFilterObject.minRangeValue;
                    var max = currentInputFilterObject.maxRangeValue;
                    if (isNaN(min) && isNaN(max)) {
                        returnValue = returnValue && true;
                    } else {
                        var currentColumnIndex = currentInputFilterObject.columnIndex;
                        if (hasDetails || allowSelect)
                            currentColumnIndex = currentColumnIndex + 1;

                        var value = parseFloat(searchData[currentColumnIndex]) || 0;
                        if ((isNaN(min) && isNaN(max)) ||
                        (isNaN(min) && value <= max) ||
                        (min <= value && isNaN(max)) ||
                        (min <= value && value <= max)) {
                            returnValue = returnValue && true;
                        } else
                            returnValue = returnValue && false;
                    }
                    return returnValue;
                }
            }

            if (rangeFilterObj.forChildTable) {
                rootTableData.childrenNotRegularFilters.push(rangeFilterObj);
                rootTableData.allChildrenFilters.push(rangeFilterObj);
            } else {
                rootTableData.allRootFilters.push(rangeFilterObj);
            }
            rootTableData.rangeFilters.push(rangeFilterObj);

            if (!isNaN(minRangeValue) || !isNaN(minRangeValue)) {
                if (!rangeFilterObj.forChildTable) {
                    if ($.inArray(rangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                        rootTableData.currentNotRegularFilters.push(rangeFilterObj);
                    //rootTableData.jqueryTable.draw();
                }
            }

            $minRangeFilter.keyup(function () {
                var $rangeFilter = $(this);

                var minString = $rangeFilter.val();
                var minValue = parseFloat(minString, 10);
                if (rangeFilterObj.minRangeValue !== minValue) {
                    rangeFilterObj.minRangeValue = minValue;

                    if (rangeFilterObj.forChildTable) {
                        if ($.inArray(rangeFilterObj, rootTableData.currentNotRegularChildrenFilters) === -1)
                            rootTableData.currentNotRegularChildrenFilters.push(rangeFilterObj);
                        drawChildren();
                    } else {
                        if ($.inArray(rangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                            rootTableData.currentNotRegularFilters.push(rangeFilterObj);
                        drawParent();
                    }
                }
            });

            $maxRangeFilter.keyup(function () {
                var $rangeFilter = $(this);

                var maxString = $rangeFilter.val();
                var maxValue = parseFloat(maxString, 10);
                if (rangeFilterObj.maxRangeValue !== maxValue) {
                    rangeFilterObj.maxRangeValue = maxValue;

                    if (rangeFilterObj.forChildTable) {

                        if ($.inArray(rangeFilterObj, rootTableData.currentNotRegularChildrenFilters) === -1)
                            rootTableData.currentNotRegularChildrenFilters.push(rangeFilterObj);

                        drawChildren();
                    } else {
                        if ($.inArray(rangeFilterObj, rootTableData.currentNotRegularFilters) === -1)
                            rootTableData.currentNotRegularFilters.push(rangeFilterObj);
                        drawParent();
                    }
                }
            });
        }
    });

    $dom.find(".boolean-filter .controls").each(function (idx, obj) {
        var $booleanFilterGroup = $(this);
        var $filtersContainer = $booleanFilterGroup.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");
        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $booleanFilterGroup.parents(".table-filter-container:first").data("for-child-table");

            var inputs = $booleanFilterGroup.find("input[type='checkbox']");
            var $trueFilter = $(inputs[0]);
            var $falseFilter = $(inputs[1]);
            var trueFilterDataValue = $trueFilter.data("value");
            var falseFilterDataValue = $falseFilter.data("value");

            //Todo: Poner el index en el grupo
            var columnIndex = parseInt($trueFilter.data("index"));

            var defaultData = $booleanFilterGroup.data('default-value');
            if (defaultData) {
                $trueFilter.prop("checked", true);
            } else {
                $falseFilter.prop("checked", true);
            }

            var trueFilterValue = $trueFilter.is(":checked");
            var falseFilterValue = $falseFilter.is(":checked");

            var booleanFilterObj = {
                type: "BooleanFilter",
                $booleanFilterGroup: $booleanFilterGroup,
                $trueFilter: $trueFilter,
                trueFilterValue: trueFilterValue,
                trueFilterDataValue: trueFilterDataValue,
                $falseFilter: $falseFilter,
                falseFilterValue: falseFilterValue,
                falseFilterDataValue: falseFilterDataValue,
                forChildTable: forChildTable,
                defaultData: defaultData,
                columnIndex: columnIndex,
                columnName: $trueFilter.data("name"),
                getValue: function () {
                    var currentInputFilterObject = this;

                    if (currentInputFilterObject.trueFilterValue && currentInputFilterObject.falseFilterValue) {
                        return "";
                    } else if (currentInputFilterObject.trueFilterValue) {
                        return true;

                    } else if (currentInputFilterObject.falseFilterValue) {
                        return false;
                    } else {
                        return "";
                    }
                },
                clean: function () {
                    this.$trueFilter.attr('checked', false);
                    this.$falseFilter.attr('checked', false);
                },
                filterTable: function (tableToFilter, hasDetails, allowSelect, triggerDraw) {
                    triggerDraw = triggerDraw !== false;
                    var currentInputFilterObject = this;
                    var columnId = currentInputFilterObject.columnIndex;
                    if (hasDetails || allowSelect)
                        columnId = columnId + 1;

                    if (currentInputFilterObject.trueFilterValue && currentInputFilterObject.falseFilterValue) {
                        tableToFilter.search("").column(columnId).search("");
                    } else if (currentInputFilterObject.trueFilterValue) {

                        if (currentInputFilterObject.trueFilterDataValue) {
                            tableToFilter.column(columnId).search(true, true, false);
                        } else {
                            tableToFilter.column(columnId).search(false, true, false);
                        }

                    } else if (currentInputFilterObject.falseFilterValue) {

                        if (currentInputFilterObject.falseFilterDataValue) {
                            tableToFilter.column(columnId).search(true, true, false);
                        } else {
                            tableToFilter.column(columnId).search(false, true, false);
                        }
                    } else {
                        tableToFilter.column(columnId).search("", true, false);
                    }

                    if (triggerDraw)
                        tableToFilter.draw();
                }
            }

            if (booleanFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(booleanFilterObj);
                rootTableData.allChildrenFilters.push(booleanFilterObj);
            } else {
                rootTableData.allRootFilters.push(booleanFilterObj);
            }
            rootTableData.booleanFilters.push(booleanFilterObj);

            if (defaultData !== "" && defaultData !== undefined) {
                if (!booleanFilterObj.forChildTable) {
                    if ($.inArray(booleanFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(booleanFilterObj);
                    booleanFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.allowSelect, rootTableData.hasDetails, false);
                } else {
                    if ($.inArray(booleanFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(booleanFilterObj);
                }
            }

            $trueFilter.on('change', function () {
                var $filter = $(this);
                booleanFilterObj.trueFilterValue = $filter.is(":checked");

                if (booleanFilterObj.forChildTable) {
                    if ($.inArray(booleanFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(booleanFilterObj);
                    $.each(rootTableData.childTables, function (key, value) {
                        var tableData = value;
                        booleanFilterObj.filterTable(tableData.table, tableData.hasDetails, tableData.allowSelect, !rootTableData.deferredDraw);
                    });
                    rootTableData.filterChildren = true;
                } else {
                    if ($.inArray(booleanFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(booleanFilterObj);
                    booleanFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, !rootTableData.deferredDraw);
                    rootTableData.filterParent = true;
                }
            });

            $falseFilter.on('change', function () {
                var $filter = $(this);
                booleanFilterObj.falseFilterValue = $filter.is(":checked");

                if (booleanFilterObj.forChildTable) {
                    if ($.inArray(booleanFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(booleanFilterObj);
                    $.each(rootTableData.childTables, function (key, value) {
                        var tableData = value;
                        booleanFilterObj.filterTable(tableData.table, tableData.hasDetails, !rootTableData.deferredDraw);
                    });
                    rootTableData.filterChildren = true;
                } else {
                    if ($.inArray(booleanFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(booleanFilterObj);
                    booleanFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, !rootTableData.deferredDraw);
                    rootTableData.filterParent = true;
                }
            });
        }
    });

    $dom.find('.input-filter input').each(function (idx, obj) {
        var $inputFilter = $(this);

        var $filtersContainer = $inputFilter.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");

        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $inputFilter.parents(".table-filter-container:first").data("for-child-table");

            var columnIndex = parseInt($inputFilter.data("index"));

            var defaultData = $inputFilter.data('default-value');
            if (defaultData != null) {
                $inputFilter.val(defaultData);
            }

            var searchValue = defaultData;

            var inputFilterObj = {
                type: "InputFilter",
                $inputFilter: $inputFilter,
                forChildTable: forChildTable,
                searchValue: searchValue,
                defaultData: defaultData,
                columnIndex: columnIndex,
                columnName: $inputFilter.data("name"),
                getValue: function () {
                    return this.searchValue;
                },
                clean: function () {
                    this.$inputFilter.val('');
                },
                filterTable: function (tableToFilter, hasDetails, allowSelect, triggerDraw) {
                    triggerDraw = triggerDraw !== false;
                    var currentInputFilterObject = this;
                    var searchValue = currentInputFilterObject.searchValue;
                    var columnId = currentInputFilterObject.columnIndex;
                    if (hasDetails || allowSelect)
                        columnId = columnId + 1;

                    if (searchValue === "" || searchValue === undefined) {
                        tableToFilter.search("").column(columnId).search("");
                    } else {
                        tableToFilter.column(columnId).search(searchValue, true, false);
                    }
                    if (triggerDraw)
                        tableToFilter.draw();
                }
            }

            if (inputFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(inputFilterObj);
            }

            if (inputFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(inputFilterObj);
                rootTableData.allChildrenFilters.push(inputFilterObj);
            } else {
                rootTableData.allRootFilters.push(inputFilterObj);
            }
            rootTableData.inputFilters.push(inputFilterObj);

            if (searchValue !== "" && searchValue !== undefined) {
                if (!inputFilterObj.forChildTable) {
                    if ($.inArray(inputFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(inputFilterObj);
                    inputFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, false);
                } else {
                    if ($.inArray(inputFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(inputFilterObj);
                }
            }

            $inputFilter.on('keyup', function () {
                var $filter = $(this);
                if (inputFilterObj.searchValue !== $filter.val()) {
                    inputFilterObj.searchValue = $filter.val();

                    if (inputFilterObj.forChildTable) {
                        if ($.inArray(inputFilterObj, rootTableData.currentChildrenFilters) === -1)
                            rootTableData.currentChildrenFilters.push(inputFilterObj);
                        $.each(rootTableData.childTables, function (key, value) {
                            var tableData = value;
                            inputFilterObj.filterTable(tableData.table, tableData.hasDetails, tableData.allowSelect, !rootTableData.deferredDraw);
                        });
                        rootTableData.filterChildren = true;
                    } else {
                        if ($.inArray(inputFilterObj, rootTableData.currentFilters) === -1)
                            rootTableData.currentFilters.push(inputFilterObj);
                        inputFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, !rootTableData.deferredDraw);
                        rootTableData.filterParent = true;
                    }
                }
            });
        }
    });

    $dom.find('.int-filter input').each(function (idx, obj) {
        var $intFilter = $(this);

        var $filtersContainer = $intFilter.parents(".data-table-filters:first");
        var relatedTableId = $filtersContainer.data("table-id");

        if (relatedTableId === rootTableData.tableId) {
            var forChildTable = $intFilter.parents(".table-filter-container:first").data("for-child-table");

            var columnIndex = parseInt($intFilter.data("index"));

            var defaultData = $intFilter.data('default-value');
            if (defaultData != null) {
                $intFilter.val(defaultData);
            }

            var searchValue = defaultData;

            var intFilterObj = {
                type: "IntFilter",
                $intFilter: $intFilter,
                forChildTable: forChildTable,
                searchValue: searchValue,
                defaultData: defaultData,
                columnIndex: columnIndex,
                columnName: $intFilter.data("name"),
                getValue: function () {
                    return this.searchValue;
                },
                clean: function () {
                    this.$intFilter.val('');
                },
                filterTable: function (tableToFilter, hasDetails, allowSelect, triggerDraw) {
                    triggerDraw = triggerDraw !== false;
                    var currentIntFilterObject = this;
                    var searchValue = currentIntFilterObject.searchValue;
                    var columnId = currentIntFilterObject.columnIndex;
                    if (hasDetails || allowSelect)
                        columnId = columnId + 1;

                    if (searchValue === "" || searchValue === undefined) {
                        tableToFilter.search("").column(columnId).search("");
                    } else {
                        tableToFilter.column(columnId).search(searchValue, false, false);
                    }
                    if (triggerDraw)
                        tableToFilter.draw();
                }
            }

            if (intFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(intFilterObj);
            }

            if (intFilterObj.forChildTable) {
                rootTableData.childrenFilters.push(intFilterObj);
                rootTableData.allChildrenFilters.push(intFilterObj);
            } else {
                rootTableData.allRootFilters.push(intFilterObj);
            }
            rootTableData.intFilters.push(intFilterObj);

            if (searchValue !== "" && searchValue !== undefined) {
                if (!intFilterObj.forChildTable) {
                    if ($.inArray(intFilterObj, rootTableData.currentFilters) === -1)
                        rootTableData.currentFilters.push(intFilterObj);
                    intFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, false);
                } else {
                    if ($.inArray(intFilterObj, rootTableData.currentChildrenFilters) === -1)
                        rootTableData.currentChildrenFilters.push(intFilterObj);
                }
            }

            $intFilter.on('keyup', function () {
                var $filter = $(this);
                if (intFilterObj.searchValue !== $filter.val()) {
                    intFilterObj.searchValue = $filter.val();

                    if (intFilterObj.forChildTable) {
                        if ($.inArray(intFilterObj, rootTableData.currentChildrenFilters) === -1)
                            rootTableData.currentChildrenFilters.push(intFilterObj);
                        $.each(rootTableData.childTables, function (key, value) {
                            var tableData = value;
                            intFilterObj.filterTable(tableData.table, tableData.hasDetails, tableData.allowSelect, !rootTableData.deferredDraw);
                        });
                        rootTableData.filterChildren = true;
                    } else {
                        if ($.inArray(intFilterObj, rootTableData.currentFilters) === -1)
                            rootTableData.currentFilters.push(intFilterObj);
                        intFilterObj.filterTable(rootTableData.jqueryTable, rootTableData.hasDetails, rootTableData.allowSelect, !rootTableData.deferredDraw);
                        rootTableData.filterParent = true;
                    }
                }
            });
        }
    });

    $dom.find(".filter-clear").each(function (idx, obj) {
        var $filter = $(this);
        var $filterDiv = $filter.parents(".panel:first").find(".data-table-filters:first");
        var relatedTableId = $filterDiv.data("table-id");
        if (relatedTableId === rootTableData.tableId) {
            rootTableData.$clearFilter = $filter;
            $filter.on('click', function () {

                $.each(rootTableData.allRootFilters, function (index, th) {
                    var filter = this;
                    filter.clean();
                });
                rootTableData.currentFilters = [];
                rootTableData.currentNotRegularFilters = [];

                rootTableData.jqueryTable.search("").columns().search("").draw();

                $.each(rootTableData.allChildrenFilters, function (index, th) {
                    var filter = this;
                    filter.clean();
                });

                rootTableData.currentChildrenFilters = [];
                rootTableData.currentNotRegularChildrenFilters = [];

                $.each(rootTableData.childTables, function (index, th) {
                    var childTableData = this;
                    childTableData.table.search("").columns().search("").draw();
                });
            });
        }
    });

    $dom.find(".filter-table").each(function (idx, obj) {
        var $filter = $(this);
        var $filterDiv = $filter.parents(".panel:first").find(".data-table-filters:first");
        var relatedTableId = $filterDiv.data("table-id");
        if (relatedTableId === rootTableData.tableId) {
            rootTableData.deferredDraw = true;
            $filter.on('click', function () {

                if ((rootTableData.currentFilters.length > 0 || rootTableData.currentNotRegularFilters.length > 0) && rootTableData.filterParent) {
                    rootTableData.filterParent = false;
                    rootTableData.jqueryTable.draw();
                }

                if ((rootTableData.currentChildrenFilters.length > 0 || rootTableData.currentNotRegularChildrenFilters.length > 0) && rootTableData.filterChildren) {
                    rootTableData.filterChildren = false;
                    $.each(rootTableData.childTables, function (index, th) {
                        var childTableData = this;
                        childTableData.table.draw();
                    });
                }
            });
        }
    });

    return rootTableData;
}

var configureDataTableControls = function ($dom) {
    $.fn.dataTable.ext.search.push(
           function (settings, searchData, index, rowData, counter) {
               var currentId = settings.nTable.getAttribute('id');
               var returnValue = true;

               if (currentId in rootTables) {
                   var tableToFilter = rootTables[currentId];
                   $(tableToFilter.currentNotRegularFilters).each(function (idx, obj) {
                       var currentFilter = this;
                       returnValue = returnValue && currentFilter.IsRowMatch(tableToFilter.jqueryTable, tableToFilter.hasDetails, tableToFilter.allowSelect, searchData, rowData);
                   });
               } else {

                   $.each(rootTables, function (key, value) {
                       if (currentId.match("^" + key)) {
                           var rootTable = value;
                           var childTableData = rootTable.childTables[currentId];

                           $(rootTable.currentNotRegularChildrenFilters).each(function (idx, obj) {
                               var currentFilter = this;
                               returnValue = returnValue && currentFilter.IsRowMatch(childTableData.table, childTableData.hasDetails, childTableData.allowSelect, searchData, rowData);
                           });
                           return false;
                       }
                   });
               }

               return returnValue;
           }
       );

    var rootTables = {};
    $dom.find('.data-table-editor').each(function (idx, obj) {
        var rootTable = setupTable($dom, $(this));
        rootTables[rootTable.tableId] = rootTable;
        rootTable.jqueryTable.draw();
    });
};

(function () {
    var $document = $(document);
    $document.ready(function () {
        configureDataTableControls($document);
    });
})();
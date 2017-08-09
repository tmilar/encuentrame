(function () {
    var buildTree = function () {
        var configTree = {
            container: "#partTreeContainer",

            nodeAlign: "LEFT",

            connectors: {
                type: 'step'
            },
            node: {
                HTMLclass: ''
            },
            rootOrientation: 'WEST',
            scrollbar: 'None',
            callback: {
                onCreateNode: function (treeNode, treeNodeDom) {


                }, // this = Tree
                onCreateNodeCollapseSwitch: function (treeNode, treeNodeDom, switchDom) { console.log('onCreateNodeCollapseSwitch'); }, // this = Tree
                onAfterAddNode: function (newTreeNode, parentTreeNode, nodeStructure) { console.log('onAfterAddNode'); }, // this = Tree
                onBeforeAddNode: function (parentTreeNode, nodeStructure) { console.log('onBeforeAddNode'); }, // this = Tree
                onAfterPositionNode: function (treeNode, nodeDbIndex, containerCenter, treeCenter) { console.log('onAfterPositionNode'); }, // this = Tree
                onBeforePositionNode: function (treeNode, nodeDbIndex, containerCenter, treeCenter) { console.log('onBeforePositionNode'); }, // this = Tree
                onToggleCollapseFinished: function (treeNode, bIsCollapsed) {

                    console.log('onToggleCollapseFinished');
                }, // this = Tree
                onAfterClickCollapseSwitch: function (nodeSwitch, event) { console.log('onAfterClickCollapseSwitch'); }, // this = TreeNode
                onBeforeClickCollapseSwitch: function (nodeSwitch, event) { console.log('onBeforeClickCollapseSwitch'); }, // this = TreeNode
                onTreeLoaded: function (rootTreeNode) { console.log('onTreeLoaded'); } // this = Tree
            }
        };

        var treeBuilder = [configTree];


        var $partNodeTemplate = _.template($('#partNodeTemplate').html());
        var $operationNodeTemplate = _.template($('#operationNodeTemplate').html());
        var $rawMaterialComponentNodeTemplate = _.template($('#rawMaterialComponentNodeTemplate').html());
        var $partComponentNodeTemplate = _.template($('#partComponentNodeTemplate').html());
        var $partTreeContainer = $("#partTreeContainer");
        var url = $partTreeContainer.data("url");
        var operationType = $partTreeContainer.data("operation-type");
        var rawMaterialType = $partTreeContainer.data("raw-material-type");
        var partType = $partTreeContainer.data("part-type");
        console.log(url);
        $.ajax({
            method: "GET",
            url: url,
        })
            .done(function (data) {
                if (data.Status) {
                    var partNode = {
                        innerHTML: $partNodeTemplate(data.Info.Part),
                        collapsable: true,
                        stackChildren: false,
                    };
                    treeBuilder.push(partNode);

                    var buildPartTree = function (partData, partParentNode) {

                        $(partData.Operations).each(function (idx, operation) {
                            var operationNode = {
                                parent: partParentNode,
                                innerHTML: $operationNodeTemplate(operation),
                                collapsable: true,
                                stackChildren: false,
                            };

                            treeBuilder.push(operationNode);

                            $(operation.Components).each(function (idx, component) {
                                var componentHtml = "";
                                if (partType === component.Type) {
                                    componentHtml = $partComponentNodeTemplate(component);
                                } else {
                                    componentHtml = $rawMaterialComponentNodeTemplate(component);
                                }
                                var componentNode = {
                                    parent: operationNode,
                                    innerHTML: componentHtml,
                                    collapsable: true,
                                    stackChildren: true,
                                };

                                treeBuilder.push(componentNode);
                                if (partType === component.Type) {
                                    componentNode.collapsed = true;
                                    buildPartTree(component, componentNode);
                                }
                            });
                        });
                    }

                    buildPartTree(data.Info.Part, partNode);

                    var tree = new Treant(treeBuilder, function () {
                        $('a.refresh').on('click', function() {
                            location.reload();
                        });
                    }, $);
                }
            })
            .fail(function () {
                console.log('error');
            });

    };

    $(document).ready(function () {
        buildTree();
        $('.modal-form-submit-sm').on('shown.bs.modal', function (e) {
            var $modal = $(this);
            var $modalBody = $modal.find(".modal-body");
            configureThisControls($modalBody);

            var $form = $modalBody.find('form');
            var $alertContainer = $form.find('.alert');
            var $createButton = $form.find('.quick-create');
            var method = $form.attr('method');
            var action = $form.attr('action');


            $createButton.off('click');
            $createButton.on('click', function () {
                if ($form.valid()) {
                    $.ajax({
                        type: method,
                        url: action,
                        data: $form.serialize(),
                        async: false,
                        cache: false,
                    }).done(function (response) {
                        if (response.Status) {
                            $modal.modal('hide');
                            location.reload();
                        } else {
                            $alertContainer.find('span').text(response.ErrorMessage);
                            $alertContainer.show();
                        }
                    });
                }
            });

        });

    });
})();
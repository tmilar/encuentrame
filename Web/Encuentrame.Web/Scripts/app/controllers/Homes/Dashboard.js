(function () {
    $(document).ready(function () {

        var $eventsAlongTheTimeChart = $("#eventsAlongTheTimeChart");
        var eventsAlongTheTimeCanvas = $eventsAlongTheTimeChart[0].getContext("2d");
        var lineUrl = $eventsAlongTheTimeChart.data('url');

        $.ajax(
            {
                url: lineUrl,
                data: {},
                async: true,
                cache: false,
                method: 'POST'
            })
            .done(function (data) {
                var lineData = {
                    labels: data.Info.Labels,
                    datasets: [
                        {
                            label: "All",
                            fillColor: "rgba(220,220,220,0.2)",
                            strokeColor: "rgba(220,220,220,1)",
                            pointColor: "rgba(220,220,220,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(220,220,220,1)",
                            data: data.Info.Data1
                        },
                        {
                            label: "InEmergency",
                            fillColor: "rgba(185,56,56,0.2)",
                            strokeColor: "rgba(185,56,56,1)",
                            pointColor: "rgba(185,56,56,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(151,187,205,1)",
                            data: data.Info.Data2
                        }
                    ]
                };

                var lineOptions = {

                    ///Boolean - Whether grid lines are shown across the chart
                    scaleShowGridLines: true,

                    //String - Colour of the grid lines
                    scaleGridLineColor: "rgba(0,0,0,.05)",

                    //Number - Width of the grid lines
                    scaleGridLineWidth: 1,

                    //Boolean - Whether to show horizontal lines (except X axis)
                    scaleShowHorizontalLines: true,

                    //Boolean - Whether to show vertical lines (except Y axis)
                    scaleShowVerticalLines: true,

                    //Boolean - Whether the line is curved between points
                    bezierCurve: true,

                    //Number - Tension of the bezier curve between points
                    bezierCurveTension: 0.4,

                    //Boolean - Whether to show a dot for each point
                    pointDot: true,

                    //Number - Radius of each point dot in pixels
                    pointDotRadius: 4,

                    //Number - Pixel width of point dot stroke
                    pointDotStrokeWidth: 1,

                    //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
                    pointHitDetectionRadius: 20,

                    //Boolean - Whether to show a stroke for datasets
                    datasetStroke: true,

                    //Number - Pixel width of dataset stroke
                    datasetStrokeWidth: 2,

                    //Boolean - Whether to fill the dataset with a colour
                    datasetFill: true,

                    //String - A legend template
                    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

                };


                var eventsAlongTheTimeChart = new Chart(eventsAlongTheTimeCanvas).Line(lineData, lineOptions);
            })
            .fail(function (err) {
                console.log(err);
            });

      /**
       * ************************************************************
       */


        var $eventsByStatusChart = $("#eventsByStatus");
        var eventsByStatusCanvas = $eventsByStatusChart[0].getContext("2d");
        var pieUrl = $eventsByStatusChart.data('url');

        $.ajax(
            {
                url: pieUrl,
                data: {},
                async: true,
                cache: false,
                method: 'POST'
            })
            .done(function (data) {
                var pieData = [];

                $.each(data.Info,
                    function (idx, elem) {
                        var item = {
                            value: elem.Value,
                            color: elem.Color,
                            highlight: elem.Highlight,
                            label: elem.Label 
                        };
                        pieData.push(item);
                    });



               

                var pieOptions = {
                    //Boolean - Whether we should show a stroke on each segment
                    segmentShowStroke: true,

                    //String - The colour of each segment stroke
                    segmentStrokeColor: "#fff",

                    //Number - The width of each segment stroke
                    segmentStrokeWidth: 2,

                    //Number - The percentage of the chart that we cut out of the middle
                    percentageInnerCutout: 0, // This is 0 for Pie charts

                    //Number - PlannedQuantity of animation steps
                    animationSteps: 100,

                    //String - Animation easing effect
                    animationEasing: "easeOutBounce",

                    //Boolean - Whether we animate the rotation of the Doughnut
                    animateRotate: true,

                    //Boolean - Whether we animate scaling the Doughnut from the centre
                    animateScale: false,

                    //String - A legend template
                    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\" style=\"list-style-type: none;\"><% for (var i=0; i<segments.length; i++){%><li><div class=\"pull-left\"><span style=\"background-color:<%=segments[i].fillColor%>; width:15px; height:15px; display: block; margin-right:5px;\"></span></div><div class=\"pull-left\"><%if(segments[i].label){%><%=segments[i].label%><%}%></div><div class=\"clearfix\"></div></li><%}%></ul>"

                };

                var eventsByStatusChart = new Chart(eventsByStatusCanvas).Pie(pieData, pieOptions);
                $("#eventsByStatusLegend").append(eventsByStatusChart.generateLegend());
            })
            .fail(function (err) {
                console.log(err);
            });
    });
})();
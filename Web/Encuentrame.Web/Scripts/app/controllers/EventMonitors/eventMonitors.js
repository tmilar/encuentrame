
var okMarkers = [];
var notOkMarkers = [];
var withoutAnswerMarkers = [];
var flagMarkers = [];

var okMarkerCluster = {};
var notOkMarkerCluster = {};
var withoutAnswerMarkerCluster = {};
var imagesFolder = "";
function initMap() {

    var $iAmOkFilter = $('#IAmOk');
    var $iAmNotOkFilter = $('#IAmNotOk');
    var $withoutAnswerFilter = $('#WithoutAnswer');
    var $lastUpdateFilter = $('#LastUpdate');
    var $clusteredFilter = $('#Clustered');

    var $iAmOkAmount = $('.i-am-ok-amount');
    var $iAmNotOkAmount = $('.i-am-not-ok-amount');
    var $withoutAnswerAmount = $('.without-answer-amount');


    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');
    var $id = $('#Id');
    var $mapContainer = $('#mapContainer');

    var positionsUrl = $('#mapContainer').data("positions-url");

    imagesFolder = $mapContainer.data("images-folder");

    var flagUrl = $mapContainer.data("icon-event");
    var flagLabel = $mapContainer.data("label-event");

    var okUrl = $mapContainer.data("icon-ok-person");
    var notOkUrl = $mapContainer.data("icon-not-ok-person");
    var withoutAnswerUrl = $mapContainer.data("icon-without-answer-person");

    var lat = parseFloat($latitude.val().replace(',', '.'));
    var lng = parseFloat($longitude.val().replace(',', '.'));

    var position = { lat: lat, lng: lng };
    var map = new google.maps.Map($mapContainer.get(0), {
        zoom: 16,
        center: position,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        streetViewControl: false,
        rotateControl: true,
        fullscreenControl: true,
        mapTypeControlOptions: {
            mapTypeIds: ['roadmap', 'satellite',
                'styled_map']
        }
    });

    map.mapTypes.set('styled_map', GetStyledClean());
    map.setMapTypeId('styled_map');

    BuildMarket(map, position, flagUrl, flagLabel, flagMarkers);

     
    var load = function () {

        $.ajax(
            {
                url: positionsUrl,
                data: { eventId: $id.val(), datetimeTo: $lastUpdateFilter.val() },
                async: false,
                cache: false,
                method: 'POST'
            })
            .done(function (data) {
                removeAllmarkers();
                $.each(data.Info,
                    function (idx, elem) {
                        var positionLocal = { lat: elem.Latitude, lng: elem.Longitude };
                        var markImageUrl = withoutAnswerUrl;
                        var markLabel = elem.Username;
                        var markers = withoutAnswerMarkers;
                        if (elem.IAmOk === 20) {
                            markImageUrl = okUrl;
                            markers = okMarkers;
                        } else if (elem.IAmOk === 10) {
                            markImageUrl = notOkUrl;
                            markers = notOkMarkers;
                        }

                        BuildMarket(map, positionLocal, markImageUrl, markLabel, markers);
                    });





                $iAmOkAmount.text(okMarkers.length);
                $iAmNotOkAmount.text(notOkMarkers.length);
                $withoutAnswerAmount.text(withoutAnswerMarkers.length);

                $iAmOkFilter.prop("checked", true);
                $iAmNotOkFilter.prop("checked", true);
                $withoutAnswerFilter.prop("checked", true);

                if ($clusteredFilter.is(":checked")) {
                    clustered(map);

                } 

            })
            .fail(function (err) {
                console.log(err);
            });
    }

  

    load();

    $lastUpdateFilter.on("dp.change",
        function () {
           

            load();

        });

    $clusteredFilter.on("change",
        function () {
            var $this = $(this);
           
                removeAllClusterers();


            $iAmOkFilter.trigger('change');
            $iAmNotOkFilter.trigger('change');
            $withoutAnswerFilter.trigger('change');
        });

    $iAmOkFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, okMarkers);
                if($clusteredFilter.is(":checked"))
                {
                    okMarkerCluster = BuildCluster(map, okMarkers, imagesFolder);
                }

            } else {
                clearMarkers(okMarkers);
                clearCluster(okMarkerCluster);
            }
        });



    $iAmNotOkFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, notOkMarkers);
                if ($clusteredFilter.is(":checked")) {
                    notOkMarkerCluster = BuildCluster(map, notOkMarkers, imagesFolder);
                }
            } else {
                clearMarkers(notOkMarkers);
                clearCluster(notOkMarkerCluster);
            }
        });

    $withoutAnswerFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, withoutAnswerMarkers);
                if ($clusteredFilter.is(":checked")) {
                    withoutAnswerMarkerCluster = BuildCluster(map, withoutAnswerMarkers, imagesFolder);
                }
            } else {
                clearMarkers(withoutAnswerMarkers);
                clearCluster(withoutAnswerMarkerCluster);
            }
        });



}

function clustered(map) {
    okMarkerCluster = BuildCluster(map, okMarkers, imagesFolder);
    notOkMarkerCluster = BuildCluster(map, notOkMarkers, imagesFolder);
    withoutAnswerMarkerCluster = BuildCluster(map, withoutAnswerMarkers, imagesFolder);
}

function removeAllClusterers() {
   
    clearCluster(okMarkerCluster);
    clearCluster(notOkMarkerCluster);
    clearCluster(withoutAnswerMarkerCluster);
}

function removeAllmarkers() {

    removeAllClusterers();
    clearMarkers(okMarkers);
    okMarkers = [];
    clearMarkers(notOkMarkers);
    notOkMarkers = [];
    clearMarkers(withoutAnswerMarkers);
    withoutAnswerMarkers = [];
}




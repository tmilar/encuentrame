
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

    var defaultUrl = $mapContainer.data("icon-default-person");
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
                        if ($iAmOkFilter.length === 0) {
                            markImageUrl = defaultUrl;
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

    var iAmOkFn = function (chk) {
        if (chk) {
            showMarkers(map, okMarkers);
            if ($clusteredFilter.is(":checked")) {
                okMarkerCluster = BuildCluster(map, okMarkers, imagesFolder);
            }

        } else {
            clearMarkers(okMarkers);
            clearCluster(okMarkerCluster);
        }
    }
    var iAmNotOkFn=function(chk) {
        if (chk) {
            showMarkers(map, notOkMarkers);
            if ($clusteredFilter.is(":checked")) {
                notOkMarkerCluster = BuildCluster(map, notOkMarkers, imagesFolder);
            }
        } else {
            clearMarkers(notOkMarkers);
            clearCluster(notOkMarkerCluster);
        }
    }

    var withoutAnswerFn = function (chk) {
        if (chk) {
            showMarkers(map, withoutAnswerMarkers);
            if ($clusteredFilter.is(":checked")) {
                withoutAnswerMarkerCluster = BuildCluster(map, withoutAnswerMarkers, imagesFolder);
            }
        } else {
            clearMarkers(withoutAnswerMarkers);
            clearCluster(withoutAnswerMarkerCluster);
        }
    }

    $clusteredFilter.on("change",
        function () {
            var $this = $(this);
           
            removeAllClusterers();

            if ($iAmOkFilter.length > 0) {
                $iAmOkFilter.trigger('change');
            } else {
                iAmOkFn(true);
            }
            if ($iAmNotOkFilter.length > 0) {
                $iAmNotOkFilter.trigger('change');
            } else {
                iAmNotOkFn(true);
            }
            if ($withoutAnswerFilter.length > 0) {
                $withoutAnswerFilter.trigger('change');
            } else {
                withoutAnswerFn(true);
            }
        
           
        });

    $iAmOkFilter.on("change",
        function () {
            var $this = $(this);
            iAmOkFn($this.is(":checked"));
        });



    $iAmNotOkFilter.on("change",
        function () {
            var $this = $(this);
            iAmNotOkFn($this.is(":checked"));
        });

    $withoutAnswerFilter.on("change",
        function () {
            var $this = $(this);
            withoutAnswerFn($this.is(":checked"));
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

$(document).ready(function () {
    var $lastUpdateFilter = $('#LastUpdate');
    var $eventStartDate = $('#BeginDateTime');
    var $eventEndDate = $('#EndDateTime');
    $lastUpdateFilter.data("DateTimePicker").options( { minDate: moment($eventStartDate.val()), maxDate: moment($eventEndDate.val())});
    
});

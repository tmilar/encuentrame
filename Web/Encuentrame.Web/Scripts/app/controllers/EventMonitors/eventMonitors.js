
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


    var load= function() {
        removeAllmarkers();
        $.post(positionsUrl, { eventId: $id.val(), datetimeTo: $lastUpdateFilter.val() })
            .done(function(data) {

                $.each(data.Info,
                    function(idx, elem) {
                        var position = { lat: elem.Latitude, lng: elem.Longitude };
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

                        BuildMarket(map, position, markImageUrl, markLabel, markers);
                    });

               

                okMarkerCluster = BuildCluster(map, okMarkers);
                notOkMarkerCluster = BuildCluster(map, notOkMarkers);
                withoutAnswerMarkerCluster = BuildCluster(map, withoutAnswerMarkers);


                $iAmOkFilter.prop("checked", true);;
                $iAmNotOkFilter.prop("checked", true);;
                $withoutAnswerFilter.prop("checked", true);;

            })
            .fail(function(err) {
                console.log(err);
            });
    }

    load();

    $lastUpdateFilter.on("dp.change",
        function() {
            clearMarkers(map, withoutAnswerMarkers);
            withoutAnswerMarkers = [];
            clearMarkers(map, okMarkers);
            okMarkers = [];
            clearMarkers(map, notOkMarkers);
            notOkMarkers = [];

            load();

        });

    $iAmOkFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, okMarkers);
                okMarkerCluster = BuildCluster(map, okMarkers);
             
            } else {
                clearMarkers(okMarkers);
                okMarkerCluster.clearMarkers();
                okMarkerCluster = {};
            }
        });

    $iAmNotOkFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, notOkMarkers);
                notOkMarkerCluster = BuildCluster(map, notOkMarkers);
               
            } else {
                clearMarkers(notOkMarkers);
                notOkMarkerCluster.clearMarkers();
                notOkMarkerCluster = {};
            }
        });

    $withoutAnswerFilter.on("change",
        function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                showMarkers(map, withoutAnswerMarkers);
                withoutAnswerMarkerCluster = BuildCluster(map, withoutAnswerMarkers);
            } else {
                clearMarkers(withoutAnswerMarkers);
                withoutAnswerMarkerCluster.clearMarkers();
                withoutAnswerMarkerCluster = {};
            }
        });
}


function removeAllmarkers() {
    clearMarkers(okMarkers);
    okMarkers = [];
    if (okMarkerCluster.clearMarkers != undefined) {
        okMarkerCluster.clearMarkers();
        okMarkerCluster = {};
    }
   

    clearMarkers(notOkMarkers);
    notOkMarkers = [];
    if (notOkMarkerCluster.clearMarkers != undefined) {
        notOkMarkerCluster.clearMarkers();
        notOkMarkerCluster = {};
    }

    clearMarkers(withoutAnswerMarkers);
    withoutAnswerMarkers = [];
    if (withoutAnswerMarkerCluster.clearMarkers != undefined) {
        withoutAnswerMarkerCluster.clearMarkers();
        withoutAnswerMarkerCluster = {};

    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers(markers) {
    setMapOnAll(null, markers);
}

// Shows any markers currently in the array.
function showMarkers(map,markers) {
    setMapOnAll(map, markers);
}

function setMapOnAll(map, markers) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

function BuildCluster(map, markers) {
    var options = {
        imagePath: imagesFolder
    };

    return new MarkerClusterer(map, markers, options);
   
}

function BuildMarket(map, position, icon, title, collection) {
      var marker = new google.maps.Marker({
        map: map,
        position: position,
        icon: icon,
        title: title
    });
    collection.push(marker);
}

function GetStyledClean() {
    return new google.maps.StyledMapType([
        {
            "featureType": "administrative",
            "elementType": "geometry",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "poi",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "road",
            "elementType": "labels.icon",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "transit",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        }
    ], { name: 'Limpio' });
}


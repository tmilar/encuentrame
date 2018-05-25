

var eventMarkers = [];
var pathMarkers = [];

var imagesFolder = "";


function initMap() {

    var $eventId = $('#EventId');
    var $userId = $('#UserId');
    var $mapContainer = $('#mapContainer');
    var positionsUrl = $('#mapContainer').data("positions-url");
    var pointImageUrl = $mapContainer.data("icon-point");

    var eyeImageUrl = $mapContainer.data("icon-eye");
    var iAmOkEnumTranslations = $mapContainer.data('i-am-ok-enum');

    var $latitude = $('#EventLatitude');
    var $longitude = $('#EventLongitude');

    var flagUrl = $mapContainer.data("icon-event");
    var flagLabel = $mapContainer.data("label-event");
    imagesFolder = $mapContainer.data("images-folder");
    

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

   

    BuildMarket(map, position, flagUrl, flagLabel, eventMarkers);


    var load = function () {

        $.ajax(
            {
                url: positionsUrl,
                data: { eventId: $eventId.val(), userId: $userId.val() },
                async: false,
                cache: false,
                method: 'POST'
            })
            .done(function (data) {

                var waypoints = [];
                var seenWaypoints = [];
                $.each(data.Info.Positions,
                    function (idx, elem) {
                        var positionLocal = { lat: elem.Latitude, lng: elem.Longitude };

                        waypoints.push(positionLocal);

                        BuildMarket(map, positionLocal, pointImageUrl, moment(elem.Datetime).format("dddd, Do [de] MMMM [de] YYYY, h:mm:ss a") + ' | ' +  moment(elem.Datetime).fromNow(), pathMarkers);
                    });

                $.each(data.Info.SeenPositions,
                    function (idx, elem) {
                        var positionLocal = { lat: elem.Latitude, lng: elem.Longitude };

                        seenWaypoints.push(positionLocal);

                        BuildMarket(map, positionLocal, eyeImageUrl, moment(elem.When).format("dddd, Do [de] MMMM [de] YYYY, h:mm:ss a") + ' | ' + moment(elem.When).fromNow() + ' | ' + iAmOkEnumTranslations[elem.Status] , pathMarkers);
                    });

               
                var path = new google.maps.Polyline({
                    path: waypoints,
                    geodesic: true,
                    strokeColor: '#337ab7',
                    strokeOpacity: 0.8,
                    strokeWeight: 3
                });

                path.setMap(map);
                

            })
            .fail(function (err) {
                console.log(err);
            });
    }



    load();


}


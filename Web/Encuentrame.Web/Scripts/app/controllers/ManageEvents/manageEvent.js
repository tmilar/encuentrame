
var markers = [];
function initMap() {
    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');
    var $mapContainer = $('#mapContainer');

    var flagUrl = $mapContainer.data("icon-event");
    var position = { lat: -34.397, lng: 150.644 };


    var map = new google.maps.Map($mapContainer.get(0), {
        zoom: 8,
        center: position,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        streetViewControl: false,
        rotateControl: true,
        fullscreenControl: true
    });

  

    if ($latitude.val() !== '') {
        position.lat = parseFloat($latitude.val().replace(',', '.'));
        position.lng = parseFloat($longitude.val().replace(',', '.'));
        var marker = new google.maps.Marker({
            map: map,
            position: position,
            icon: flagUrl
        });
        markers.push(marker);
        map.setZoom(16);
        map.setCenter(position);
    }
    var geocoder = new google.maps.Geocoder();

    $('#geocoder').on('click', function () {
        geocodeAddress(geocoder, map);
    });
}

function geocodeAddress(geocoder, resultsMap) {
    var $street = $('#Street');
    var $number = $('#Number');
    var $city = $('#City');
    var $province = $('#Province');
    var $country = $('#Country');
    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');

    var address = $street.val() + ' ' + $number.val() + ', ' + $city.val() + ', ' + $province.val() + ', Argentina';
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status === 'OK') {
            resultsMap.setCenter(results[0].geometry.location);
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(null);
            }
            markers = [];

            var marker = new google.maps.Marker({
                map: resultsMap,
                position: results[0].geometry.location
            });
            markers.push(marker);
            resultsMap.setZoom(16);

            var latlng = results[0].geometry.location.toJSON();
            $latitude.val(latlng.lat.toString().replace(".", ","));
            $longitude.val(latlng.lng.toString().replace(".", ","));

        } else {
            console.log('Geocode was not successful for the following reason: ' + status);
        }
    });
}



var markers = [];
function initMap() {
    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');

    var coordinates = { lat: -34.397, lng: 150.644 };
    var map = new google.maps.Map(document.getElementById('mapContainer'), {
        zoom: 8,
        center: coordinates
    });

    if ($latitude.val() !== '') {
        coordinates.lat = parseFloat($latitude.val().replace(',', '.'));
        coordinates.lng = parseFloat($longitude.val().replace(',', '.'));
        var marker = new google.maps.Marker({
            map: map,
            position: coordinates
        });
        markers.push(marker);
        map.setZoom(16);
        map.setCenter(coordinates);
    }
    var geocoder = new google.maps.Geocoder();

    document.getElementById('geocoder').addEventListener('click', function () {
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


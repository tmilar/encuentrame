
var markers = [];
function initMap() {
    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');


    var lat = parseFloat($latitude.val().replace(',', '.'));
    var lng = parseFloat($longitude.val().replace(',', '.'));

    var coordinates = { lat: lat, lng: lng };
    var map = new google.maps.Map(document.getElementById('mapContainer'), {
        zoom: 16,
        center: coordinates
    });

}



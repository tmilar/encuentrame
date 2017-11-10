
var markers = [];
function initMap() {
    var $latitude = $('#Latitude');
    var $longitude = $('#Longitude');
    var $mapContainer = $('#mapContainer');

    var flagUrl = $mapContainer.data("icon-event");
    var flagLabel = $mapContainer.data("label-event");

    var okUrl = $mapContainer.data("icon-ok-person");
    var notOkUrl = $mapContainer.data("icon-not-ok-person");
    var withoutAnswerUrl = $mapContainer.data("icon-without-ok-person");

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

    BuildMarket(map, position, flagUrl, flagLabel);

    //Llamar a un metodo que me devuelva todas las personas y si contestaron o no. Cuando no se esta en emergencia son todos azules. 
    //Le paso en evento como parametro. {fullname, id, iamok, lastupdatepostion, position } filtro por lastupdatepostion menor que, iamok uno u otro o algunos, evento es fijo siempre
}

function BuildMarket(map, position, icon, title) {
      var marker = new google.maps.Marker({
        map: map,
        position: position,
        icon: icon,
        title: title
    });
    markers.push(marker);
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


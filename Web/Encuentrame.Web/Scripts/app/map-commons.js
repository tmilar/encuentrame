function clearCluster(markerCluster) {
    if (markerCluster.clearMarkers != undefined) {
        markerCluster.clearMarkers();
    }
    markerCluster = {};
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers(markers) {
    setMapOnAll(null, markers);
}

// Shows any markers currently in the array.
function showMarkers(map, markers) {
    setMapOnAll(map, markers);
}

function setMapOnAll(map, markers) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

function BuildCluster(map, markers, imgFolder) {
    var options = {
        imagePath: imgFolder
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
/// <reference path="global.js" />
/// <reference path="_references.js" />

window.googleGeocoder;

$(function () {
    $('#Address').on('blur', function (e) {
        getLatLongFromAddress(e);
    });
});

function getLatLongFromAddress(event) {
    window.googleGeocoder = new google.maps.Geocoder();
    var text;
    var address = $('#Address').val(),
        inputLat = $('#Lat'),
        inputLng = $('#Lng'),
        inputLog = $('#LogMessages');
    address = address.replace(/([\r\n]+|\n|\r)/gm, ' ');
    address = address.replace(/(\s+)/g, ' ');
    console.log(address);

    window.googleGeocoder.geocode({ 'address': address }, function (results, status) {
        console.dir(results);
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var position = results[0].geometry.location;
                inputLat.val(position.lat());
                inputLng.val(position.lng());
                inputLog.val(ko.toJSON(ko.toJS(results)));
                console.log(ko.toJS(results));
            } else {
                text = "According to Google Maps, this location does not exist. \nPlease enter a valid location.";
                alert(text);
                inputLog.val(text);
            }
        } else {
            text = 'Geocode was not successful for the following reason: ' + status;
            alert(text);
            inputLog.val(text);
        }
    });
}


function showMap(latitude, longitude, msg) {
    var map_convas = document.getElementById("map"),
        myLatLng = new google.maps.LatLng(latitude, longitude),
        myOptions = {
            center: myLatLng,
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.HYBRID
        },
        map = new google.maps.Map(map_convas, myOptions);
    console.dir(map);
    var marker = new google.maps.Marker({
        map: map,
        position: myLatLng,
        animation: google.maps.Animation.Drop,
        title: msg || 'Testing Location'
    });
    marker.setMap(map);
}


function getMultiLocations(locations) {
    var geocoder = new google.maps.Geocoder(),
        canvas = document.getElementById("map"),
        markers = [],
        _lat = 32.78,
        _lng = -96.8;
    geocoder.geocode({ 'address': '98004' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var latlng = results[0].geometry.location;
            _lat = latlng.lat();
            _lng = latlng.lng();
        }
    });
    var myOptions = {
        zoom: 8,
        center: new google.maps.LatLng(_lat, _lng),
        mapTypeId: google.maps.MapTypeId.HYBRID
    };
    var map = new google.maps.Map(canvas, myOptions);

    $.each(locations, function (index, location) {
        if (location.lat !== null) {
            setTimeout(function () {
                markers.push(new google.maps.Marker({
                    map: map,
                    position: new google.maps.LatLng(location.lat, location.lng),
                    animation: google.maps.Animation.DROP,
                    title: location.address,
                    zIndex: index
                }));
            }, index * 200);
        }
    });
}

var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 40.2, lng: -8.4166667 },
        zoom: 8
    });
}
$(document).ready(function () {

    var bingMap;

    LoadMap();


    function sleep(milliseconds) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if ((new Date().getTime() - start) > milliseconds) {
                break;
            }
        }
    }

    // called automatically by loading page
    function LoadMap() {

        // create the map
        MAP = new Microsoft.Maps.Map(document.getElementById('map'), {
            credentials: 'AqL6HufCTSGkky_IGwoIynFD1auBGW89-WwrxMCOnrXP0aiuArJlzZpunPQUOXzn',
            center: new Microsoft.Maps.Location(53.360641, -6.251250),
            mapTypeId: Microsoft.Maps.MapTypeId.aerial,
            zoom: 16,
            showLocateMeButton: false,
            showMapTypeSelector: false,
            showScalebar: false,
            showDashboard: false
        });

        // load devices to show pushpins
        $.ajax({
            url: "/api/Device",
            type: "GET",
            dataType: "json",
            success: DevicesRetrieved
        });

        /// create a click event handler
        Microsoft.Maps.Events.addHandler(MAP, 'click', function () { highlight('mapClick'); });

        // do something on click
        function highlight(id) {
            document.getElementById(id).style.background = 'LightGreen';
            setTimeout(function () { document.getElementById(id).style.background = 'white'; }, 1000);
        }

        function DevicesRetrieved(data) {

            for (var i = 0, len = data.length; i < len; i++) {
                var device = data[i];
                try {

                    var deviceNumber = new String(device.DeviceName);
                    deviceNumber = deviceNumber.replace("Gateway", "");
                    deviceNumber = deviceNumber.trim();

                    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(device.Longitude, device.Latitude), { text: deviceNumber, title: device.Stand });
                    MAP.entities.push(pushpin);
                } catch (e) { }

            }

          //  map.show();
           // progress.hide();
        }
    }
});
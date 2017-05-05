$(document).ready(function () {

    // this is the number of minutes to calculate maximum values for the realtime chart
    var realtimeMinutes = 20;
    var rolltimeMinutes = 20;

    // the date to show data for on the realtime chart
    var realtimeDate = null;
    var rolltimeDate = null;

    // populate the charts
    PopulateCharts();

    function PopulateCharts() {
        PopulateRealTimeChart();
        PopulateRollingAVGChart();
    }

    function GetMinutesLabel(minutes) {
        switch (minutes) {
            case 20:
                return "20 minutes";
                break;
            case 60:
                return "1 Hour";
                break;
            case 120:
                return "2 Hours";
                break;
            case 180:
                return "3 Hours";
                break;
            case 1440:
                return "1 Day";
                break;
        }
    }

    /// Populates the realtime chart in top right shoiwing latest values from all gateways and max values over defined time period
    function PopulateRealTimeChart() {

        if (realtimeMinutes > 0) {
            $.ajax({
                url: "/api/RealTimeSoundData?minutes=" + realtimeMinutes,
                type: "GET",
                dataType: "json",
                success: RealTimeSoundDataRetrieved
            });
        }
        else {
            $.ajax({
                url: "/api/RealTimeDateSoundData?date=" + realtimeDate,
                type: "GET",
                dataType: "json",
                success: RealTimeSoundDataRetrieved
            });
        }


        function RealTimeSoundDataRetrieved(soundData) {
            var barWidth = 0.4;
            if (soundData != null && soundData.length > 0) {
                try {
                    var element = document.getElementById('realTimeChart');
                    var series1 = [];
                    var series2 = [];
                    var ticks = [];

                    for (var i = 0; i < soundData.length; i++) {
                        var deviceId = soundData[i].DeviceId;
                        var deviceData = soundData[i].Device;

                        var deviceSoundData = soundData[i].LatestSoundData
                        var maxDeviceSoundData = soundData[i].MaximumSoundData;
                        var minDeviceSoundData = soundData[i].MinimumSoundData;

                        if (minDeviceSoundData) {
                            var point1 = [i + 1, minDeviceSoundData.LAMax];
                            series1.push(point1);

                            var label1 = [i + 1, deviceData.ShortTitle + " - Min<p>" + minDeviceSoundData.TimeLabelShort + "</p>"];
                            ticks.push(label1);
                        }

                        if( deviceSoundData) {
                            var point1 = [i + 1, deviceSoundData.LAMax];
                            series1.push(point1);

                            var label1 = [i + 1, deviceData.ShortTitle + " - Live<p>" + deviceSoundData.TimeLabelShort + "</p>"];
                            ticks.push(label1);
                        }

                        if (maxDeviceSoundData) {
                            var point2 = [i + 1 + barWidth, maxDeviceSoundData.LAMax];
                            series2.push(point2);

                            var label2 = [i + 1 + barWidth, deviceData.ShortTitle + " - Max<p>" + maxDeviceSoundData.TimeLabelShort + "</p>"];
                            ticks.push(label2);
                        }

                    }

                    var series = [
                        { data: series1, label: 'Latest Sound Data (LAMax)', color: '#00FF00' },
                    ];

                    if (maxDeviceSoundData) {
                        if (minDeviceSoundData) {
                            series = [
                             { data: series1, label: 'Minimum Sound Data (LAMax)', color: '#00FF00' },
                             { data: series2, label: 'Maximum Sound Data (LAMax)', color: '#FF0000' },
                            ];
                        }
                        else {
                            series = [
                              { data: series1, label: 'Latest Sound Data (LAMax)', color: '#00FF00' },
                              { data: series2, label: 'Maximum Sound Data (LAMax)', color: '#FF0000' },
                            ];
                        }
                    }

                    // This function prepend each label with 'y = '
                    function labelFn(label) {
                        return " - " + label;
                    }

                    function defaultTrackFormatter(xValue) {
                        return xValue.y;
                    }

                    $('#realTimeChart').show();

                    Flotr.draw(element, series, {
                        bars: {
                            show: true,
                            shadowSize: 0,
                            barWidth: barWidth,
                            align: 'right'
                        },
                        markers: {
                            show: true,
                            position: 'ct',
                            fontSize: 11,
                        },
                        legend: {
                            position: 'se',            // Position the legend 'south-east'.
                            labelFormatter: labelFn,   // Format the labels.
                            backgroundColor: '#D2E8FF' // A light blue background color.
                        },
                        mouse: {
                            track: true,		// => true to track mouse
                            relative: true,		// => position to show the track value box
                            trackFormatter: defaultTrackFormatter,	// => fn: int -> string
                            margin: 3,		// => margin for the track value box
                            color: '#ff3f19',	// => color for the tracking points, null to hide points
                            trackDecimals: 1,	// => number of decimals for track values
                            radius: 3,		// => radius of the tracking points
                            sensibility: 2		// => the smaller this value, the more precise you've to point with the mouse
                        },

                        yaxis: {
                            min: 30,
                            // max: 80,
                            autoscaleMargin: 10
                        },
                        xaxis: {
                            ticks: ticks,
                            relative: true
                        },
                        grid: {
                            horizontalLines: true,
                            verticalLines: false,
                        },
                        subtitle: 'The LAMax is the loudest level reached in each 1 minute time slot. This might be a very short event that only lasts for a fraction of a second. ',
                    });

                }
                catch (e) {
                    if (e.message)
                        alert('charts.js: RealTimeSoundDataRetrieved:' + e.message);
                    else
                        alert('charts.js: RealTimeSoundDataRetrieved2' + e);
                }
            }
            else {
                var chartDiv = $("div#realTimeChart");
                chartDiv.html('<p style="text-align:center">Sorry no data to display, try changing the filter condition</p>');
            }

            $('#realTimeChart').show();
            $('#progressRealtimeChart').hide();

       //     setTimeout(PopulateRealTimeChart, 10000);
        }

    }

   

    function PopulateRollingAVGChart() {
        if (rolltimeMinutes == 0 && rolltimeDate != null) {
            $.ajax({
                url: "/api/RollingAverageDateSoundData?date=" + rolltimeDate,
                type: "GET",
                dataType: "json",
                success: RollingAVGDataRetrieved
            });
        }
        else {
            $.ajax({
                url: "/api/RollingAverageSoundData?minutes=" + rolltimeMinutes,
                type: "GET",
                dataType: "json",
                success: RollingAVGDataRetrieved
            });
        }


        function RollingAVGDataRetrieved(deviceData) {

            var element = document.getElementById("rollingAveragesChart");
            var rollingseries = [];
            var ticks = [];

            try {
                if (deviceData != null && deviceData.length > 0) {
                    // there will be an object for each device each with an array of data for the sound data
                    // not all devices will be transmitting data at all times
                    // so build the chart structure using the device with most data
                    var soundDataWithMostpoints = null;
                    for (var i = 0; i < deviceData.length; i++) {
                        if (soundDataWithMostpoints == null) {
                            soundDataWithMostpoints = deviceData[i].SoundData;
                        }
                        else if (deviceData[i].SoundData.length > soundDataWithMostpoints.length) {
                            soundDataWithMostpoints = deviceData[i].SoundData;
                        }
                    }

                    // set up max 10 ticks for the graph using the device with most data
                    var indexCount = (soundDataWithMostpoints.length < 10 ? soundDataWithMostpoints.length : 10);
                    var indexDelta = Math.floor(soundDataWithMostpoints.length / indexCount);
                    var threshholddata = [];
                    for (var bar = 1; bar <= soundDataWithMostpoints.length - 1; bar += indexDelta) {
                        var point = [];
                        point = [bar, soundDataWithMostpoints[bar - 1].TimeLabelShort];
                        ticks.push(point);
                        var sound = 75;
                        var point = [bar, 75];
                        threshholddata.push(point);
                    }
                    rollingseries.push({ data: threshholddata, color: 'Red', points: { show: true, radius: 1 } });

                    var colors = ["Green", "Blue", "Black", "Yellow", "Purple"];

                    for (var i = 0; i < deviceData.length; i++) {

                        // get the device object as we will want some data from it
                        var device = deviceData[i].Device;

                        try {

                            // get the sound data for the device
                            var deviceSoundData = deviceData[i].SoundData;

                            // get the chart data for this device
                            var chartData = [];
                            for (var foo = 0; foo < deviceSoundData.length; foo++) {
                                var sound = deviceSoundData[foo];
                                var point = [foo + 1, sound.AVGPressure];
                                chartData.push(point);
                            }

                            rollingseries.push({ data: chartData, color: colors[i], label: device.Description + ' - ' + device.Stand, lines: { fill: false } });
                        }
                        catch (e) {
                            alert(e.message);
                        }
                    }


                    function labelAVGFn(label) {
                        return " - " + label;
                    }

                    // need to show now so chart works
                    $('#rollingAveragesChart').show();

                    // Load all the data in one pass; if we only got partial
                    // data we could merge it with what we already have.
                    Flotr.draw(element, rollingseries, {
                        xaxis: {
                            mode: 'time',
                            ticks: ticks,
                            labelsAngle: 45
                        },
                        yaxis: {
                            //  min: 30,

                        },
                        grid: {
                            minorVerticalLines: true
                        },
                        mouse: {
                            track: true,		// => true to track mouse
                            relative: true,		// => position to show the track value box
                            margin: 3,		// => margin for the track value box
                            color: '#ff3f19',	// => color for the tracking points, null to hide points
                            trackDecimals: 1,	// => number of decimals for track values
                            radius: 3,		// => radius of the tracking points
                            sensibility: 2		// => the smaller this value, the more precise you've to point with the mouse
                        },
                        subtitle: 'The LAeq is the average sound level in each measurement period. Our measurement equipment gives us 1 minute averages, however, noise limits for events are based on 15 minute averages. So we take all the readings and process them to get an average for the last 15 minute period. Each point on the graph represents the average sound level measured over the previous 15 minutes. So the graph is not showing you the raw live readings, it is showing you processed readings that can be compared directly to noise limits.',
                        legend: {
                            position: 'sw',            // Position the legend 'south-east'.
                            labelFormatter: labelAVGFn,   // Format the labels.
                            backgroundColor: '#D2E8FF' // A light blue background color.
                        },
                    });
                }
                else {
                    var chartDiv = $("div#rollingAveragesChart");
                    chartDiv.html('<p style="text-align:center">Sorry no data to display, try changing the filter condition</p>');
                }
            }
            catch (ex) {
                var chartDiv = $("div#rollingAveragesChart");
                chartDiv.html('<p style="text-align:center">Sorry an error occurred, please try again</p><p>' + ex.message + '</p>');
            }
            finally {
                $('#rollingAveragesChart').show();
                $('#progressRollingChart').hide();
            }

            setTimeout(PopulateRollingAVGChart, 10000);
        }
    }



    try {

        $('#realtimeDatePicker').datepicker({
            format: "dd MM yyyy",
            weekStart: 1,
            todayBtn: "linked",
            autoclose: true,
            todayHighlight: true
        });


        $('#realtimeDatePicker').on("changeDate", function () {
            realtimeMinutes = 0;
            realtimeDate = $('#realtimeDatePicker').datepicker('getFormattedDate');
            $('#realtimecharttitle').html(' for ' + realtimeDate);
            $('#realTimeChart').hide();
            $('#progressRealtimeChart').show();
            PopulateRealTimeChart();
        });

        $('#rolltimeDatePicker').datepicker({
            format: "dd MM yyyy",
            weekStart: 1,
            todayBtn: "linked",
            autoclose: true,
            todayHighlight: true
        });


        $('#rolltimeDatePicker').on("changeDate", function () {
            alert('here');
            rolltimeMinutes = 0;
            rolltimeDate = $('#rolltimeDatePicker').datepicker('getFormattedDate');
            $('#rollingcharttitle').html(' for ' + rolltimeDate);
            $('#rollingAveragesChart').hide();
            $('#progressRollingChart').show();
            PopulateRollingAVGChart();
        });

        // set up handlers for the realtime drop down filter
        $("#realtime20Minutes").click(realtimeMinutes, function () {
            realtimeDate = null;
            if (realtimeMinutes != 20) {
                realtimeMinutes = 20;
                $('#realtimecharttitle').html('for the last 20 minutes');
                $('#realTimeChart').hide();
                $('#progressRealtimeChart').show();
                PopulateRealTimeChart();
            }
        });

        $("#realtime60Minutes").click(realtimeMinutes, function () {
            realtimeDate = null;
            if (realtimeMinutes != 60) {
                realtimeMinutes = 60;
                $('#realtimecharttitle').html('for the last hour');
                $('#realTimeChart').hide();
                $('#progressRealtimeChart').show();
                PopulateRealTimeChart();
            }
        });

        $("#realtime1440Minutes").click(realtimeMinutes, function () {
            realtimeDate = null;
            if (realtimeMinutes != 1440) {
                realtimeMinutes = 1440;
                $('#realtimecharttitle').html('for the last day');
                $('#realTimeChart').hide();
                $('#progressRealtimeChart').show();
                PopulateRealTimeChart();
            }
        });

        $("#realtime120Minutes").click(realtimeMinutes, function () {
            realtimeDate = null;
            if (realtimeMinutes != 120) {
                realtimeMinutes = 120;
                $('#realtimecharttitle').html('for the last 2 hours');
                $('#realTimeChart').hide();
                $('#progressRealtimeChart').show();
                PopulateRealTimeChart();
            }
        });

        $("#realtime180Minutes").click(realtimeMinutes, function () {
            realtimeDate = null;
            if (realtimeMinutes != 180) {
                realtimeMinutes = 180;
                $('#realtimecharttitle').html('for the last 3 hours');
                $('#realTimeChart').hide();
                $('#progressRealtimeChart').show();
                PopulateRealTimeChart();
            }
        });

        // set up handlers for the realtime drop down filter
        $("#rolltime20Minutes").click(rolltimeMinutes, function () {
            rolltimeDate = null;
            if (rolltimeMinutes != 20) {
                rolltimeMinutes = 20;
                $('#rollingcharttitle').html('for the last 20 minutes');
                $('#rollingAveragesChart').hide();
                $('#progressRollingChart').show();
                PopulateRollingAVGChart();
            }
        });

        $("#rolltime60Minutes").click(rolltimeMinutes, function () {
            rolltimeDate = null;
            if (rolltimeMinutes != 60) {
                rolltimeMinutes = 60;
                $('#rollingcharttitle').html('for the last hour');
                $('#rollingAveragesChart').hide();
                $('#progressRollingChart').show();
                PopulateRollingAVGChart();
            }
        });

        $("#rolltime1440Minutes").click(rolltimeMinutes, function () {
            rolltimeDate = null;
            if (rolltimeMinutes != 1440) {
                rolltimeMinutes = 1440;
                $('#rollingcharttitle').html('for the last day');
                $('#rollingAveragesChart').hide();
                $('#progressRollingChart').show();
                PopulateRollingAVGChart();
            }
        });

        $("#rolltime120Minutes").click(rolltimeMinutes, function () {
            rolltimeDate = null;
            if (rolltimeMinutes != 120) {
                rolltimeMinutes = 120;
                $('#rollingcharttitle').html('for the last 2 hours');
                $('#rollingAveragesChart').hide();
                $('#progressRollingChart').show();
                PopulateRollingAVGChart();
            }
        });

        $("#rolltime180Minutes").click(rolltimeMinutes, function () {
            rolltimeDate = null;
            if (rolltimeMinutes != 180) {
                rolltimeMinutes = 180;
                $('#rollingcharttitle').html('for the last 3 hours');
                $('#rollingAveragesChart').hide();
                $('#progressRollingChart').show();
                PopulateRollingAVGChart();
            }
        });
    }
    catch (e) {
        alert('charts.js: events' + e.message);
    }
        
});
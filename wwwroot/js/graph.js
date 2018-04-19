
var sdata;
var humidityLine;
var temperatureLine;
var soilMoistureLine;
var lightLine;

(function () {

    //initilize Humidity Chart
    init_H_T_Chart();

    //initilize Temperature Chart
    initSoilChart();

    //initilize Temperature Chart
    initLightChart();

    //initilize signal R
    initSignalR();

})();


function initLightChart(){
    var lightChart = new SmoothieChart({
        maxValue:100,
        minValue:0,
        grid: {
            strokeStyle: 'rgb(125, 0, 0)', fillStyle: 'rgb(60, 0, 0)',
            lineWidth: 1, millisPerLine: 250, verticalSections: 6,
        },
        labels:{fontSize:20},
        tooltip: true,
        timestampFormatter: SmoothieChart.timeFormatter
    });

    lightChart.streamTo(document.getElementById("light"));

    lightLine = new TimeSeries();
    lightChart.addTimeSeries(lightLine,
        { strokeStyle: 'rgb(0, 255, 255)', fillStyle: 'rgba(0, 255, 255, 0.7)', lineWidth: 3 });
}


function init_H_T_Chart(){

    var envChart = new SmoothieChart({
        maxValue:100,
        minValue:0,
        grid: {
            strokeStyle: 'rgb(125, 0, 0)', fillStyle: 'rgb(60, 0, 0)',
            lineWidth: 1, millisPerLine: 250, verticalSections: 6,
        },
        labels:{fontSize:20},
        tooltip: true,
        timestampFormatter: SmoothieChart.timeFormatter
    });
  
    envChart.streamTo(document.getElementById("env"));

    // Data
    humidityLine = new TimeSeries();
    temperatureLine = new TimeSeries();

    envChart.addTimeSeries(humidityLine,
        { strokeStyle: 'rgb(0, 255, 0)', fillStyle: 'rgba(0, 255, 0, 0.4)', lineWidth: 3 });
    envChart.addTimeSeries(temperatureLine,
        { strokeStyle: 'rgb(255, 0, 255)', fillStyle: 'rgba(255, 0, 255, 0.3)', lineWidth: 3 });

}

function initSoilChart(){
    var soilChart = new SmoothieChart({
        maxValue:1000,
        minValue:0,
        grid: {
            strokeStyle: 'rgb(125, 0, 0)', fillStyle: 'rgb(60, 0, 0)',
            lineWidth: 1, millisPerLine: 250, verticalSections: 6,
        },
        labels:{fontSize:20},
        tooltip: true,
        timestampFormatter: SmoothieChart.timeFormatter
    });

    soilChart.streamTo(document.getElementById("soil"));

    soilMoistureLine = new TimeSeries();
    soilChart.addTimeSeries(soilMoistureLine,
        { strokeStyle: 'rgb(0, 255, 255)', fillStyle: 'rgba(0, 255, 255, 0.8)', lineWidth: 3 });
}




function initSignalR() {
    
    let transportType = signalR.TransportType.LongPolling;
    let http = new signalR.HttpConnection(`http://${document.location.host}/hubs`, { transport: transportType });
    var connection = new signalR.HubConnection(http);

    //const connection = new signalR.HubConnection('/hubs');

    connection.on('PushSensorData', (sensorData) => {
        sdata = sensorData;
        humidityLine.append(new Date().getTime(), sensorData.humidity);
        temperatureLine.append(new Date().getTime(), sensorData.temperature);
        //soilMoistureLine.append(new Date().getTime(), sensorData.pressure);
        lightLine.append(new Date().getTime(), sensorData.light);
    });

    connection.on('PushSoilSensorData', (sensorData) => {
        soilMoistureLine.append(new Date().getTime(), sensorData);
    });

    // document.getElementById('send').addEventListener('click', event => {
    //     connection.invoke('function_name',arg1, arg2).catch(err => showErr(err));
    //     event.preventDefault();
    // });

    function showErr(msg) {
        const listItem = document.createElement('li');
        listItem.setAttribute('style', 'color: red');
        listItem.innerText = msg.toString();
        document.getElementById('messages').appendChild(listItem);
    }

    connection.start().catch(err => showErr(err));
}
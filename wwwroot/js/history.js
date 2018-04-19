var chart;
(function () {
    
    $('#date1').datepicker({
        onSelect: function(date) {
            makeChart(date);
         },    
    });
    
    $('#date1').click(function(){
        $('#date1').datepicker('show');
    });
    
    chart = Morris.Line({
        autoSkip: false,
        element: 'env',
        data: [],
        xkey: 'timeStamp',
        ykeys: ['humidity', 'light', 'pressure', 'soilMoisture', 'temperature'],
        labels: ['humidity', 'SunLight', 'Pressure', 'SoilMoisture', 'Temperature']
      });
})();

var dat;

function makeChart(e){
    $.ajax({
        url: "/api/History?date=" + e,
        cache: false,
        success: function(data){
            chart.setData(data);
            dat = data;
            chart.redraw();
        }
      });
  }
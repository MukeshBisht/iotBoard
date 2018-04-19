var dat;
$( document ).ready(function() {
    $.ajax({
        url: "/api/Weather/current",
        cache: false,
        success: function(data){
        dat = data;
        document.getElementById("c_name").innerHTML = data.place;
        document.getElementById("temp").innerHTML = data.temp + "&#8451;";
        document.getElementById("wind").innerHTML = "Wind : " + data.windSpeed + " mpH";
        document.getElementById("clouds").innerHTML = "Clouds : " + data.clouds + "%";
        document.getElementById("details").innerHTML = data.weather + " | " + data.description;
        document.getElementById("wimg").src= "/images/" + data.icon + ".svg";
        }
      });

      $.ajax({
        url: "/api/Weather/forecast",
        cache: false,
        success: function(data){
            //dat = data;
            data.forEach(element => {     
                var s = "/images/" + element.icon + ".svg";
                var img = "<img width=40 height=40 src='"+s+"'/>";
                var r = "<tr><td>";
                r += (new Date(element.date)) + "</td><td>"; 
                r += element.weather + "</td><td>"; 
                r += element.description + "</td><td>";
                r += element.temp +  "&#8451;</td><td>"; 
                r += element.minTemp + "&#8451;</td><td>"; 
                r += element.maxTemp + "&#8451;</td><td>";         
                r +=  img + "</td><td>";       
                r += element.clouds + "</td><td>";    
                r += element.windSpeed + "</td><td>";   
                r += "</tr>";        
                var row = $(r);
                $("#forecastTable > tbody").append(row);
            });
        }
      });

});


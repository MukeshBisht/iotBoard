using System;
using System.Collections.Generic;

namespace IOT.Models
{

    // all classes for opneweathermap

    public class City {
     public string name { get; set; }   
     public string country { get; set; }  
     public Coord coord { get; set; }  
    }
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class WeatherDetail
    {
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public long dt { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }


    public class WeatherForecast {
        public int cnt { get; set; }
        public List<Forecast> list { get; set; }
        public City city { get; set; }
    }

    public class Forecast {
        public long dt { get; set; }
        public Main main {get;set;}
        public List<Weather> weather { get; set; }
        public string dt_txt { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
    }
}
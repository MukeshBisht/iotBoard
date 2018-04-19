
using System;
using System.Collections.Generic;
using System.Net;
using IOT.Models;
using Newtonsoft.Json;

namespace IOT.BL
{
    public class WeatherFacade
    {
        private WeatherAPI API;

        public WeatherFacade(WeatherAPI api)
        {
            API = api;
        }

        public Condition GetCurrentWeatherDetail()
        {
            Condition weatherCondition = null;
            WeatherDetail wd = new WeatherDetail();
            string url = API.API + "/weather?lat=" + API.Latitude + "&lon=" + API.Longitude + "&units=metric&APPID=" + API.Key;
           // Console.WriteLine(url);
            using (var client = new WebClient())
            {
                try{
                    var json = client.DownloadString(url);
                    wd = JsonConvert.DeserializeObject<WeatherDetail>(json);
                    weatherCondition = fromWeatherDetail(wd);
                }catch {
                    Console.WriteLine("failed to fetch weather data");
                    return null;
                }
            }
            return weatherCondition;
        }

        //forecasting
        public List<Condition> GetForecast()
        {
            List<Condition> forecast = new List<Condition>();
            WeatherForecast wd = new WeatherForecast();
            string url = API.API + "/forecast?lat=" + API.Latitude + "&lon=" + API.Longitude + "&units=metric&APPID=" + API.Key;
            //Console.WriteLine(url);
            using (var client = new WebClient())
            {
                try{
                var json = client.DownloadString(url);
                wd = JsonConvert.DeserializeObject<WeatherForecast>(json);
                forecast = formWeatherForecast(wd);
                }catch {
                    Console.WriteLine("failed to fetch forecast data");
                    return null;
                }
            }
            return forecast;
        }

        private Condition fromWeatherDetail(WeatherDetail wd){
            DateTime datetime= UnixTimestampToDateTime(wd.dt);
            return new Condition(){
                    place = wd.name,
                    weather = wd.weather[0].main,
                    description = wd.weather[0].description,
                    icon = wd.weather[0].icon,
                    temp = wd.main.temp,
                    minTemp = wd.main.temp_min,
                    maxTemp = wd.main.temp_max,
                    windSpeed = wd.wind.speed,
                    clouds = wd.clouds.all,
                    date = datetime
                };
        }

        private List<Condition> formWeatherForecast(WeatherForecast weatherForecast){
            List<Condition> forecast = new List<Condition>();
            foreach (var wf in weatherForecast.list)
            {
                DateTime datetime= UnixTimestampToDateTime(wf.dt);
                Condition con = new Condition(){
                    place = weatherForecast.city.name,
                    weather = wf.weather[0].main,
                    description = wf.weather[0].description,
                    icon = wf.weather[0].icon,
                    temp = wf.main.temp,
                    minTemp = wf.main.temp_min,
                    maxTemp = wf.main.temp_max,
                    windSpeed = wf.wind.speed,
                    clouds = wf.clouds.all,
                    date = datetime
                };
                forecast.Add(con);
            }
            return forecast;
        }

        public static DateTime UnixTimestampToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
        }
    }
}
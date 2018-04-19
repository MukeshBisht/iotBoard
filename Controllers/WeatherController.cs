using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.BL;
using IOT.DB;
using IOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace app.Controllers
{
    [Route("api/[controller]/")]
    public class WeatherController : Controller
    {
        private IOptions<WeatherAPI> API;
        private readonly AppDB appDB;
        public WeatherController(IOptions<WeatherAPI> APISettings, AppDB appDB)
        {
            API = APISettings;
            this.appDB = appDB;
        }

        [Route("current")]
        [HttpGet]
        public async Task<Condition> GetCurrentAsync()
        {
            WeatherFacade weatherFacade = new WeatherFacade(API.Value);
            Condition weather = null;
            using (appDB){
                WeatherRepository repo = new WeatherRepository(appDB);
                var cons = await repo.LatestConditionAsync();
                if(cons.Count>0){
                    weather = cons[0];
                    TimeSpan span = (DateTime.Now - weather.timeStamp);
                    if(span.Minutes < 10){
                        Console.WriteLine("db");
                        return weather;
                    }
                }
                Console.WriteLine("api");
                var w = weatherFacade.GetCurrentWeatherDetail();
                if(w != null){
                    await repo.InsertAsync(w);
                    weather = w;
                }
            }
            return weather;
        }

        [Route("forecast")]
        [HttpGet]
        public async Task<List<Condition>> GetForecastAsync()
        {
            WeatherFacade weatherFacade = new WeatherFacade(API.Value);
            List<Condition> weather = new List<Condition>();

            using (appDB){
                WeatherRepository repo = new WeatherRepository(appDB);
                weather = await repo.LatestConditionAsync(true);
                if(weather.Count>0){
                    var con = weather[0];
                    TimeSpan span = (DateTime.Now - con.timeStamp);
                    if(span.Minutes < 30){
                        Console.WriteLine("db");
                        //myList.OrderBy(x => x.Created).ToList();
                        return weather.OrderBy(x=>x.date).ToList();
                    }
                }
                Console.WriteLine("api");
                var w = weatherFacade.GetForecast();
                if(w != null ){
                    await repo.InsertAsync(w, true);
                    weather = w;
                }

                var wa = weather.Where( c=> c.weather.Trim() =="Rain").First();
                if(wa != null){
                    IOT.Models.Action act = new IOT.Models.Action(){
                        Text = "Weather forecast predicts " + wa.description + " on " + wa.date ,
                        Act = 0
                    };
              
                    ActionRepository repo2 = new ActionRepository(appDB);
                    await repo2.InsertAsync(act);
                }

            }          
            return weather.OrderBy(x=>x.date).ToList();;
        }
    }
}
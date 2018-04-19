using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOT.Models;
using IOT.BL;
using Microsoft.AspNetCore.SignalR;
using IOT.DB;
using System;
using System.Collections.Generic;

namespace IOT.Controllers
{
    public class TelemetryController : Controller
    {
        private IHubContext<Telemetry> HubContext { get; set; }
        private readonly AppDB appDB;
        public TelemetryController(IHubContext<Telemetry> hubcontext, AppDB appDB)
        {
            HubContext = hubcontext;
            this.appDB = appDB;
        }

        [Route("Telemetry")]
        public IActionResult Graph()
        {
            return View();
        }

        [Route("Telemetry/History")]
        public IActionResult History()
        {
            return View();
        }

        [Route("api/Telemetry")]
        [HttpPost]
        public async Task<string> PostAsync([FromBody]SensorData data)
        {
            data.Optimize();
            
            await HubContext.Clients.All.SendAsync("PushSensorData", data);
            using (appDB)
            {
                SensorDataRepository Srepo = new SensorDataRepository(appDB);
                await Srepo.InsertAsync(data);
            
            if (data.SoilMoisture > 50)
            {              
                    Models.Action act = new Models.Action(){
                        Text = "Farm will need water soon ",
                        Act = 4
                    };

                    if(data.SoilMoisture > 70){
                        act.Text = "farm needs watering ";
                        act.Act = 3;
                    }
                    ActionRepository repo = new ActionRepository(appDB);
                    await repo.InsertAsync(act);
                }
            }

            return "OK";
        }


        [HttpPost("api/Telemetry/{value}")]
        public async Task<string> PostMoistureAsync(double value)
        {
            var val = value / 10;
            await HubContext.Clients.All.SendAsync("PushSoilSensorData", val);
            if (val > 50)
            {
                using (appDB)
                {
                    Models.Action act = new Models.Action(){
                        Text = "Farm will need water soon ",
                        Act = 4
                    };

                    if(val > 70){
                        act.Text = "farm needs watering ";
                        act.Act = 3;
                    }
                    ActionRepository repo = new ActionRepository(appDB);
                    await repo.InsertAsync(act);
                }
            }
            return "OK";
        }

        [Route("api/History")]
        [HttpGet]
        public async Task<List<SensorData>> getSensorData(DateTime date)
        {
            using (appDB)
            {
                //DateTime dt = Convert.ToDateTime(date.ToString("yyyy-dd-MM"));
                SensorDataRepository repo = new SensorDataRepository(appDB);
                return await repo.TelemetryAsync(date);
            }
        }
    }
}


using System;
using System.Threading.Tasks;
using IOT.Models;
using Microsoft.AspNetCore.SignalR;

namespace IOT.BL {

 
    public class Telemetry : Hub {
        
        public Task SendMessage()
        {
            return Clients.All.SendAsync("PushSensorData", new SensorData());
        }
    }
}
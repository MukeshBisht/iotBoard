using System;
using IOT.DB;
using Newtonsoft.Json;

namespace IOT.Models
{
    public class SensorData
    {
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Light { get; set; }
        public double SoilMoisture { get; set; }
        public double Altitude { get; set; }
        public DateTime TimeStamp { get; set; }


        public void Optimize(){

            this.Light = this.Light/10;
            this.SoilMoisture = this.SoilMoisture/10;
            //conver Pa to kPa (kiloPascal)
            this.Pressure = this.Pressure/1000;
        }
    }
}
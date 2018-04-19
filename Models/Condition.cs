using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public class Condition {
        public int id { get; set; }
        public string weather { get; set; }     
        public string description { get; set; }
        public string icon { get; set; }
        public DateTime date { get; set; }
        public string place { get; set; }
        public double clouds { get; set; }
        public double temp { get; set; }
        public double minTemp { get; set; }
        public double maxTemp { get; set; }
        public double windSpeed { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
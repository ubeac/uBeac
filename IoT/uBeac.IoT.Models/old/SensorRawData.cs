using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class SensorRawData
    {
        public string SensorUid { get; set; }
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public int Unit { get; set; }
        public int Prefix { get; set; }
        public Dictionary<string, decimal> Data { get; set; }
    }
}

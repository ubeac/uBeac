using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class SensorRawData
    {
        public string SensorUid { get; set; }
        public DateTime DateTime { get; set; }
        public SensorType1 Type { get; set; }
        public MeasurementUnit Unit { get; set; }
        public MeasurementUnitPrefix Prefix { get; set; }
        public Dictionary<string, decimal> Data { get; set; }
    }
}

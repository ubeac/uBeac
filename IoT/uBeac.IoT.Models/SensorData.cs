using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class SensorData
    {
        public Guid SensorId { get; set; }
        public Guid GatewayId { get; set; }
        public DateTime DateTime { get; set; }
        public Dictionary<string, decimal> Data { get; set; }
    }
}

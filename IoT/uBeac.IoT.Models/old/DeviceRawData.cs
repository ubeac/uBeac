using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class DeviceRawData 
    {
        public string Uid { get; set; }
        public Guid GatewayId { get; set; }
        public DateTime DateTime { get; set; }
        public List<SensorRawData> Sensors { get; set; }
        public bool IsValid { get; set; }        
    }
}

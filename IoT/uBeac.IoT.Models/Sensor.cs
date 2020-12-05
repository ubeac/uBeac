using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Sensor : BaseEntity
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public SensorType Type { get; set; }
        public string Description { get; set; }        
        public bool Persist { get; set; }
        public List<string> Schema { get; set; }
    }
}

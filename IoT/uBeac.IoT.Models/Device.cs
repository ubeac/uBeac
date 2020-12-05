using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Device : BaseEntity
    {
        public string Name { get; set; }
        public Guid TeamId { get; set; }
        public string Uid { get; set; }
        public string Description { get; set; }
        public DateTime? LastRequest { get; set; }
        public long RequestCount { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? BuildingId { get; set; }
        public Coordinate Coordinate { get; set; }
        public List<Sensor> Sensors { get; set; }
    }
}

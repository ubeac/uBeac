using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class Device : BaseTeamEntity
    {
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

using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Building : BaseEntity
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public Coordinate Coordinate { get; set; }
        public List<BuildingZone> Zones { get; set; }
    }
}

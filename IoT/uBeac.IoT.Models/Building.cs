using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public class Building : BaseTeamEntity
    {
        public string Description { get; set; }
        public Address Address { get; set; }
        public Coordinate Coordinate { get; set; }
        public List<BuildingZone> Zones { get; set; }
    }
}

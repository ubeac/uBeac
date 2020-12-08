using System;

namespace uBeac.IoT.Models
{
    public class BuildingZone : BaseTeamEntity
    {
        public Guid BuildingId { get; set; }
        public string Description { get; set; }
        public Guid PlanFileId { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}

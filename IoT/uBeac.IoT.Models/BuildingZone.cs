using System;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class BuildingZone : BaseEntity
    {
        public string Name { get; set; }
        public Guid BuildingId { get; set; }
        public string Description { get; set; }
        public Guid PlanFileId { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}

using System;

namespace uBeac.IoT.Models
{
    public partial class BuildingZone
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PlanFileId { get; set; }

        public virtual Building Building { get; set; }
        public virtual File File { get; set; }
    }
}

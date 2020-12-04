using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class BuildingZone : IEntity
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

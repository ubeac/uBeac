using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class File: IEntity
    {
        public File()
        {
            BuildingZones = new HashSet<BuildingZone>();
        }

        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public virtual ICollection<BuildingZone> BuildingZones { get; set; }
    }
}

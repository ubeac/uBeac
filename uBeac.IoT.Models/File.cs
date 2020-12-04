using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public partial class File
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

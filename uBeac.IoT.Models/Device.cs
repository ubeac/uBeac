using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public partial class Device
    {
        public Device()
        {
            Sensors = new HashSet<Sensor>();
        }

        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? BuildingZoneId { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}

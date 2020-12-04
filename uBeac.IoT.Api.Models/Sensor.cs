using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class Sensor : IEntity
    {
        public Sensor()
        {
            SensorSchemas = new HashSet<SensorSchema>();
        }

        public Guid Id { get; set; }
        public string Uid { get; set; }
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Prefix { get; set; }
        public int Unit { get; set; }
        public string SchemaJSON { get; set; }

        public virtual Device Device { get; set; }
        public virtual ICollection<SensorSchema> SensorSchemas { get; set; }
    }
}

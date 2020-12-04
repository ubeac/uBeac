using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public partial class SensorSchema
    {
        public SensorSchema()
        {
            SensorSchemaValues = new HashSet<SensorSchemaValue>();
        }

        public Guid Id { get; set; }
        public Guid SensorId { get; set; }
        public string Name { get; set; }

        public virtual Sensor Sensor { get; set; }
        public virtual ICollection<SensorSchemaValue> SensorSchemaValues { get; set; }
    }
}

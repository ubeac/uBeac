using System;
using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class SensorSchema : IEntity
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

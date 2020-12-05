using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class SensorSchemaValue : IEntity
    {
        public Guid Id { get; set; }
        public Guid SensorSchemaId { get; set; }
        public Guid GatewayId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public virtual Gateway Gateway { get; set; }
        public virtual SensorSchema SensorSchema { get; set; }
    }
}

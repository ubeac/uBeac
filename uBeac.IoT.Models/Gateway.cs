using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{
    public partial class Gateway
    {
        public Gateway()
        {
            GatewayDatas = new HashSet<GatewayData>();
            GatewayHttpHeaders = new HashSet<GatewayHttpHeader>();
            GatewayIPRestrictions = new HashSet<GatewayIPRestriction>();
            SensorSchemaValues = new HashSet<SensorSchemaValue>();
        }

        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string Uid { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? BuildingZoneId { get; set; }
        public bool HttpEnabled { get; set; }
        public bool HttpSecureOnly { get; set; }
        public bool MqttEnabled { get; set; }
        public string MqttUsername { get; set; }
        public string MqttPassword { get; set; }
        public bool MqttSecureOnly { get; set; }

        public virtual ICollection<GatewayData> GatewayDatas { get; set; }
        public virtual ICollection<GatewayHttpHeader> GatewayHttpHeaders { get; set; }
        public virtual ICollection<GatewayIPRestriction> GatewayIPRestrictions { get; set; }
        public virtual Product Product { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<SensorSchemaValue> SensorSchemaValues { get; set; }
    }
}

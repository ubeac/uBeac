using System;

namespace uBeac.IoT.Models
{
    public partial class GatewayHttpHeader
    {
        public Guid Id { get; set; }
        public System.Guid GatewayId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Gateway Gateway { get; set; }
    }
}

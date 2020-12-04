using System;

namespace uBeac.IoT.Models
{
    public partial class GatewayIPRestriction
    {
        public Guid Id { get; set; }
        public Guid GatewayId { get; set; }
        public string IP { get; set; }
        public bool Allowed { get; set; }

        public virtual Gateway Gateway { get; set; }
    }
}

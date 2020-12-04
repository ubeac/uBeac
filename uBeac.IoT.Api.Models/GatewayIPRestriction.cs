using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class GatewayIPRestriction: IEntity
    {
        public Guid Id { get; set; }
        public Guid GatewayId { get; set; }
        public string IP { get; set; }
        public bool Allowed { get; set; }

        public virtual Gateway Gateway { get; set; }
    }
}

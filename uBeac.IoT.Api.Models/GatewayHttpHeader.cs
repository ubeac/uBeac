using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class GatewayHttpHeader : IEntity
    {
        public Guid Id { get; set; }
        public Guid GatewayId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Gateway Gateway { get; set; }
    }
}

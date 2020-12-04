using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class GatewayData : IEntity
    {
        public Guid Id { get; set; }
        public string TraceId { get; set; }
        public string Body { get; set; }
        public Guid GatewayId { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] Bytes { get; set; }
        public string Url { get; set; }
        public string Protocol { get; set; }
        public string Method { get; set; }

        public virtual Gateway Gateway { get; set; }
    }
}

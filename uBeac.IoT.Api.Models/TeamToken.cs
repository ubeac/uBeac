using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class TeamToken : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TeamId { get; set; }
        public string Token { get; set; }

        public virtual Team Team { get; set; }
    }
}

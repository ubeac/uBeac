using System;
using uBeac.Common;

namespace uBeac.IoT.Api.Models
{
    public partial class TeamUser : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}

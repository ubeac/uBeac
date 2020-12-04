using System;

namespace uBeac.IoT.Models
{
    public partial class TeamUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}

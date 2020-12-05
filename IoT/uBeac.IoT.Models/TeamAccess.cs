using System;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class TeamAccess : BaseEntity
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}

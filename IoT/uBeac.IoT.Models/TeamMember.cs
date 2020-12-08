using System;

namespace uBeac.IoT.Models
{
    public class TeamMember : BaseTeamEntity
    {
        public Guid UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}

using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Description { get; set; }
        public List<TeamMember> Members { get; set; }
    }
}
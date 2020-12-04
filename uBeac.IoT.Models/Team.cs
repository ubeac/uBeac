using System;
using System.Collections.Generic;

namespace uBeac.IoT.Models
{    
    public partial class Team
    {
        public Team()
        {
            Buildings = new HashSet<Building>();
            Devices = new HashSet<Device>();
            Gateways = new HashSet<Gateway>();
            TeamTokens = new HashSet<TeamToken>();
            TeamUsers = new HashSet<TeamUser>();
        }
    
        public Guid Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Gateway> Gateways { get; set; }
        public virtual ICollection<TeamToken> TeamTokens { get; set; }
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
    }
}

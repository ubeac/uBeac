using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Description { get; set; }
        public List<Building> Buildings { get; set; }
        public List<Gateway> Gateways { get; set; }
        public List<Device> Devices { get; set; }
        public List<TeamAccess> Accesses { get; set; }
        public List<UserProfile> Users { get; set; }
        public List<TeamToken> Tokens { get; set; }
    }
}
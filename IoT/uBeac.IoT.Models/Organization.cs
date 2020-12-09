using System.Collections.Generic;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Team> Teams { get; set; }
    }
}

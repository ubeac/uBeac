using System;
using uBeac.Common;

namespace uBeac.IoT.Models
{
    public abstract class BaseTeamEntity : BaseEntity
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }
}

using System;

namespace uBeac.IoT.Models
{
    public partial class TeamToken
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TeamId { get; set; }
        public string Token { get; set; }
    
        public virtual Team Team { get; set; }
    }
}
